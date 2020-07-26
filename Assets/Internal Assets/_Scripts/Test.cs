using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Topology;
using UnityEngine;


public enum ColorSetting
{
    HeightGradient,
    Random
}



[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter),typeof(MeshCollider))]
public class Test : MonoBehaviour
{


    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public float displayRadius = 1;
    public int rejectionSamples = 30;
    List<Vector2> points;

    // private void OnValidate()
    // {
    //     points  = PoissonDiscSampling.GeneratePoints(radius,regionSize,rejectionSamples);
    // }

    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(regionSize/2, regionSize);

    //     if(points != null)
    //     {
    //         foreach (var point in points)
    //         {
    //             Gizmos.DrawSphere(point, displayRadius);
    //         }
    //     }
    // }

    [Header("General Settings")]
    [Range(1,1000)] public int sizeX;
    [Range(1,1000)] public int sizeY;

    [Header("PointDistribution")]
    [Range(4,6000)] public int pointDensity;

    [Header("Simple Perlin Noise")]
    public float seed = 0f;
    [Range(1,1000)]
    public float heightScale = 1;
    [Range(1,1000)]
    public float scale = 1;
    [Range(1,1000)]
    public float dampening = 0.15f;
    [Header("Layered Noise")]
    [Range(1,1000)]
    public float octaves = 1;
    [Range(1,1000)]
    public float persistence = 0.4f;
    [Range(1,1000)]
    public float lacunarity = 3f;
    public Vector2 offset;

    [Header("Color Settings")]
    public ColorSetting colorSetting;
    public Gradient heightGradient;

    
    private Polygon polygon;
    private TriangleNet.Mesh mesh;
    private UnityEngine.Mesh terrainMesh;
    private List<float> heights = new List<float>();
    private float minNoiseHeight;
    private float maxNoiseHeight;

    void Update()
    {
         if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("INITIATE");
            Initiate();
        }
    }

    public void Initiate()
    {
        polygon = new Polygon();

        for (int i = 0; i < pointDensity; i++)
        {
            var x = Random.Range(.0f,  sizeX);
            var y = Random.Range(.0f, sizeY);

            polygon.Add(new Vertex(x,y));
        }

        ConstraintOptions constraints = new ConstraintOptions();
        constraints.ConformingDelaunay = true;


        mesh = polygon.Triangulate() as TriangleNet.Mesh;

        points  = PoissonDiscSampling.GeneratePoints(radius,regionSize,rejectionSamples);
        Debug.Log(points.Count);
        ShapeTerrain();
        GenerateMesh();
    }

    public void ShapeTerrain()
    {
        minNoiseHeight = float.PositiveInfinity;
        maxNoiseHeight = float.NegativeInfinity;

        for (int i = 0; i < mesh.vertices.Count; i++)
        {
            float amplitude = 1f;
            float frequency = 1f;
            float noiseHeight = 0f;

            for (int o = 0; o < octaves; o++)
            {
                float xValue = (float) mesh.vertices[i].x / scale * frequency;
                float yValue = (float) mesh.vertices[i].y / scale * frequency;

                float perlinValue = Mathf.PerlinNoise(xValue + offset.x + seed, yValue + offset.y + seed)*2-1;
                perlinValue *= dampening;

                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            if(noiseHeight > maxNoiseHeight)
            {
                maxNoiseHeight = noiseHeight;
            }
            else if(noiseHeight < minNoiseHeight)
            {
                minNoiseHeight = noiseHeight;
            }

            noiseHeight = (noiseHeight<0f)? noiseHeight*heightScale/10f : noiseHeight * heightScale;

            heights.Add(noiseHeight);

        }
    }
    public void GenerateMesh()
    {
         List<Vector3> vertices = new List<Vector3>();
         List<Vector3> normals = new List<Vector3>();
         List<Vector2> uvs = new List<Vector2>();
         List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

         IEnumerator<Triangle> triangleEnum = mesh.Triangles.GetEnumerator();

        for (int i = 0; i < mesh.Triangles.Count; i++)
        {
            if(!triangleEnum.MoveNext())
            {
                break;
            }

            Triangle currentTriangle = triangleEnum.Current;

            Vector3 v0 = new Vector3((float)currentTriangle.vertices[2].x, heights[currentTriangle.vertices[2].id], (float) currentTriangle.vertices[2].y);
            Vector3 v1 = new Vector3((float)currentTriangle.vertices[1].x, heights[currentTriangle.vertices[1].id], (float) currentTriangle.vertices[1].y);
            Vector3 v2 = new Vector3((float)currentTriangle.vertices[0].x, heights[currentTriangle.vertices[0].id], (float) currentTriangle.vertices[0].y);


            triangles.Add(vertices.Count);
            triangles.Add(vertices.Count +1);
            triangles.Add(vertices.Count + 2);

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            var normal = Vector3.Cross(v1-v0,v2-v0);

            for (int x = 0; x < 3; x++)
            {
                normals.Add(normal);
                uvs.Add(Vector3.zero);
                colors.Add(EvaluateColor(currentTriangle));
            }

           

            
        }


        terrainMesh  = new Mesh();

        Vector3[] vertArray = vertices.ToArray();
        Vector3[] normArray = normals.ToArray();
        int[] triangleArray = triangles.ToArray();
        Vector2[] uvArray = uvs.ToArray();
        Color[] colorsArray = colors.ToArray();

        terrainMesh.vertices = vertArray;
        terrainMesh.triangles = triangleArray;
        terrainMesh.uv = uvArray;
        terrainMesh.normals = normArray;
        terrainMesh.colors = colorsArray;
        GetComponent<MeshFilter>().mesh = terrainMesh;

     
    }

    private Color EvaluateColor(Triangle triangle)
    {
        var currentHeight = heights[triangle.vertices[0].id] + heights[triangle.vertices[1].id] + heights[triangle.vertices[0].id];
        currentHeight /= 3f;

        switch (colorSetting)
        {
            case ColorSetting.Random:
                return new Color( Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
            case ColorSetting.HeightGradient:
                currentHeight = (currentHeight < 0f)? currentHeight/heightScale * 10f : currentHeight/ heightScale;
                var gradientVal = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, currentHeight);
                return heightGradient.Evaluate(gradientVal);
        }

        return Color.magenta;
    }
}
