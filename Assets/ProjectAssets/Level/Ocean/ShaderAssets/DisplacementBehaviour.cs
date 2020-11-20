using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO: convert to OA coding conventions.
// TODO: account for player y position

// SOURCE: https://github.com/real-marco-b/unity-water-shader2d
// Modified version
/// <summary>
/// Used to update ocean shader. 
/// </summary>
/// 
[RequireComponent(typeof(OAWindowResize))]
public class DisplacementBehaviour : MonoBehaviour 
{
    private Material _oceanMat;
    private Material _uiMat;
    private RenderTexture _screenTex;
    private RenderTexture _gameTexture;
    private RenderTexture _uiTex;
    private RenderTexture _waterMaskTex;

    public Texture _displacementTex;
    public Texture _waterTex;
    public float _turbulence = 1f;
    public float _baseHeight = 0.4f;
    public float _scrollOffset = 0f;
    public float _heightOffset = 0f;
    public Camera _uiCamera;

    private GameObject _postRenderCamObj;
    private Camera _postRenderCam;
    private Camera _screenCam;
    private LayerMask _waterMask;

    void Start() 
    {
        _screenCam = GetComponent<Camera>();

        UnityAction onResize = () =>
        {
            var width = Screen.width;
            var height = Screen.height;

            _screenTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            _screenTex.wrapMode = TextureWrapMode.Repeat;

            _gameTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            _gameTexture.wrapMode = TextureWrapMode.Repeat;

            _uiTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            _uiTex.wrapMode = TextureWrapMode.Repeat;

            _waterMaskTex = new RenderTexture(width / 4, height / 4, 0, RenderTextureFormat.R8);
            _waterMaskTex.wrapMode = TextureWrapMode.Repeat;

            _uiCamera.SetTargetBuffers(_uiTex.colorBuffer, _uiTex.depthBuffer);
            _uiCamera.enabled = false;

            _screenCam.SetTargetBuffers(_screenTex.colorBuffer, _screenTex.depthBuffer);
        };

        onResize();
        GetComponent<OAWindowResize>().OnWindowResize.AddListener(onResize);

        CreatePostRenderCam();

        _waterMask = 1 << LayerMask.NameToLayer("Water");


        var oceanShader = Shader.Find("Unlit/DisplacementShader");
        _oceanMat = new Material(oceanShader);

        var uiShader = Shader.Find("Unlit/UIShader");
        _uiMat = new Material(uiShader);
    }

    void OnPostRender()
	{
        _postRenderCam.CopyFrom(_screenCam);
        _postRenderCam.clearFlags = CameraClearFlags.SolidColor;
        _postRenderCam.backgroundColor = Color.black;
        _postRenderCam.cullingMask = _waterMask;
        _postRenderCam.targetTexture = _waterMaskTex;
        _postRenderCam.Render();

        // TODO: some of this is a waste to do every post render. Move to start.
        _oceanMat.SetTexture("_MaskTex", _waterMaskTex);
        _oceanMat.SetTexture("_DisplacementTex", _displacementTex);
        _oceanMat.SetTexture("_WaterTex", _waterTex);
        _oceanMat.SetFloat("_Turbulence", _turbulence);
        _oceanMat.SetFloat("_BaseHeight", _baseHeight);
        _oceanMat.SetFloat("_ScrollOffset", _scrollOffset);
        _oceanMat.SetFloat("_HeightOffset", _heightOffset);

        // Perform ocean post processing
        // Graphics.Blit(_screenTex, null, _oceanMat);
        Graphics.Blit(_screenTex, _gameTexture, _oceanMat);

        _uiCamera.Render();

        // Send post processed game render and ui to ui shader
        _uiMat.SetTexture("_GameTex", _gameTexture);
        _uiMat.SetTexture("_UITex", _uiTex);

        // Render the combined texture
        Graphics.Blit(_uiTex, null, _uiMat);
    }

    private void CreatePostRenderCam() 
    {
        _postRenderCamObj = new GameObject("PostRenderCam");
        // TODO: not set this as child of main camera?
        _postRenderCamObj.transform.position = transform.position;
        _postRenderCamObj.transform.rotation = transform.rotation;
        _postRenderCam = _postRenderCamObj.AddComponent<Camera>();
        _postRenderCam.enabled = false;
        _postRenderCam.orthographic = true;
    }
}
