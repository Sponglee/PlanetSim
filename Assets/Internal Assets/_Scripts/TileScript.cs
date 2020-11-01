using UnityEngine;

public partial class TileScript : MonoBehaviour
{
    public enum TileStates
    {
        Free,
        Trees,
        City,
        Occupied,
        Unwalkable
    }

    public bool WalkAble = true;
    public bool Enemy = false;



    public Vector2 offset;

    private Vector2 jumpSize;
    private Vector2 jumpStep;



    public float length = 1.6f;
    [HideInInspector()] public Vector3 difference;
    [HideInInspector()] public GameObject cam;
    public Transform contentHolder;
    public Transform unitHolder;

    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (value == false)
            {
                // UpdateTile();
            }
            isVisible = value;
        }
    }

    public Node tileNode;

    private float startPosX, startPosZ;
    private bool isVisible = true;

    //Setting up a grid values for Tile
    // public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    // {
    //     // IsEmpty = true;
    //     // this.GridPosition = gridPos;
    //     transform.position = worldPos;
    //     transform.SetParent(parent);
    //     // WorldController.Instance.Tiles.Add(gridPos, this);

    // }
    private void Awake()
    {
        tileNode = GetComponent<Node>();

    }

    void Start()
    {
        jumpSize = GridController.Instance.worldSize;
        jumpStep = jumpSize / 1.17647f;

        cam = Camera.main.gameObject;
        startPosX = transform.position.x;
        startPosZ = transform.position.z;


        UpdateTilePosition(cam.transform.position.x, 0);
        UpdateTilePosition(cam.transform.position.z, 1);
    }

    public void SetUpTileType(TileType type)
    {
        GameObject tmpContent = Instantiate(type.tileVisualPrefab, contentHolder);
        if (tmpContent.transform.childCount > 1)
            tmpContent.transform.GetChild(1).Rotate(Vector3.up, Random.Range(0f, 360f));

    }

    void FixedUpdate()
    {
        // Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam.GetComponent<Camera>());
        // if (!IsVisible && GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        // {
        //     IsVisible = true;
        // }
        // else if (IsVisible && !GeometryUtility.TestPlanesAABB(planes, GetComponentInChildren<MeshCollider>().bounds))
        // {
        //     IsVisible = false;
        // }

        difference = (cam.transform.position - transform.position + new Vector3(offset.x, 0f, offset.y));

        if (Mathf.Abs(difference.x) > jumpStep.x)
        {
            UpdateTilePosition(cam.transform.position.x, 0);
        }
        if (Mathf.Abs(difference.z) > jumpStep.y)
        {
            UpdateTilePosition(cam.transform.position.z, 1);
        }

        // if (cam.transform.position.x > 1000f || cam.transform.position.z > 1000f)
        // {
        //     cam.transform.position = new Vector3(cam.transform.position.x % 1000f, cam.transform.position.y, cam.transform.position.z % 1000f);
        //     UpdateTilePosition(cam.transform.position.x, 0);
        //     UpdateTilePosition(cam.transform.position.z, 1);
        // }

    }

    private void UpdateTile()
    {
        UpdateTilePosition(cam.transform.position.x, 0);
        UpdateTilePosition(cam.transform.position.z, 1);
    }

    //Update tiles depending on what axis is needed (0 - x, 1 - z)
    private void UpdateTilePosition(float camProjectionCoord, int axis)
    {
        //Pick a start position projection coordinate
        float calculatedStartPos = (axis == 0 ? startPosX : startPosZ);
        float camPos = camProjectionCoord;



        //Update position projection coordinate depending on axis
        if (axis == 0)
        {
            calculatedStartPos += length * (jumpSize.x) * Mathf.Sign(camProjectionCoord - transform.position.x);
            startPosX = calculatedStartPos;
            transform.position = new Vector3(calculatedStartPos % 1000f, transform.position.y, transform.position.z);
        }
        else if (axis == 1)
        {
            calculatedStartPos += length * (jumpSize.y) * Mathf.Sign(camProjectionCoord - transform.position.z);
            startPosZ = calculatedStartPos;
            transform.position = new Vector3(transform.position.x, transform.position.y, calculatedStartPos % 1000f);
        }
    }

}