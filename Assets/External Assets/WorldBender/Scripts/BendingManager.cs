using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BendingManager : MonoBehaviour
{
    #region Constants

    private const string BENDING_FEATURE = "ENABLE_BENDING";

    private const string PLANET_FEATURE = "ENABLE_BENDING_PLANET";

    private static readonly int BENDING_AMOUNT =
      Shader.PropertyToID("_BendingAmount");

    #endregion


    #region Inspector

    [SerializeField]
    private bool enablePlanet = default;
    [SerializeField]
    private bool enableBending = default;

    [SerializeField]
    [Range(0, 500)]
    private static int frustumSize = 300;

    [SerializeField]
    [Range(0f, 0.1f)]
    private float bendingAmount = 0.015f;

    #endregion


    #region Fields

    private float _prevAmount;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        if (Application.isPlaying && enableBending)
            Shader.EnableKeyword(BENDING_FEATURE);
        else
            Shader.DisableKeyword(BENDING_FEATURE);

        if (enablePlanet)
            Shader.EnableKeyword(PLANET_FEATURE);
        else
            Shader.DisableKeyword(PLANET_FEATURE);

        UpdateBendingAmount();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
            return;

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void Update()
    {
        if (Math.Abs(_prevAmount - bendingAmount) > Mathf.Epsilon)
            UpdateBendingAmount();
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    #endregion


    #region Methods

    private void UpdateBendingAmount()
    {
        _prevAmount = bendingAmount;
        Shader.SetGlobalFloat(BENDING_AMOUNT, bendingAmount);
    }

    private static void OnBeginCameraRendering(ScriptableRenderContext ctx,
                                                Camera cam)
    {
        cam.cullingMatrix = Matrix4x4.Ortho(-frustumSize, frustumSize, -frustumSize, frustumSize, 0.001f, frustumSize) *
                            cam.worldToCameraMatrix;
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx,
                                              Camera cam)
    {
        cam.ResetCullingMatrix();

    }

    #endregion
}