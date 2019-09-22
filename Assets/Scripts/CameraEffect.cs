using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    public Shader curShader;
    [Range(0.0f, 1.0f)]
    public float grayScaleAmount = 1.0f;
    private Material curMaterial;
    public Material CurMaterial
    {
        get
        {
            if (curMaterial == null)
            {
                Material mat = new Material(curShader);
                mat.hideFlags = HideFlags.HideAndDontSave;
                curMaterial = mat;
            }
            return curMaterial;
        }
    }
    // Use this for initialization
    void Start() {
        if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
    }
    void OnDisable() {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }
    void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
    {
        if (curShader != null)
        {
            CurMaterial.SetFloat("_LuminosityAmount", grayScaleAmount);
            Graphics.Blit(srcTexture, destTexture, CurMaterial);
        }
        else
        {
            Graphics.Blit(srcTexture, destTexture);
        }
    }
}