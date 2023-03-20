using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ThumbnailTaker : MonoBehaviour
{
    public int screenshotWidth = 512;
    public int screenshotHeight = 512;
    [SerializeField] private float distanceMultiplier = 1.2f;
    [SerializeField] private float heightOffsetMultiplier = .1f;
    private Camera screenshotCamera;

    private Texture2D ScreenshotTexture => _screenshotTexture ?? (_screenshotTexture = new Texture2D(screenshotWidth, screenshotHeight));
    private Texture2D _screenshotTexture;

    private void Awake()
    {
        screenshotCamera = GetComponent<Camera>();   
    }

    public void TakeScreenshot(GameObject _targetObject, string _presetName, Action _onDone)
    {
        var bounds = _targetObject.GetComponentInChildren<Renderer>().bounds;

        var position = bounds.center;
        position += Vector3.forward * bounds.extents.magnitude * distanceMultiplier;
        position += Vector3.left * bounds.extents.magnitude;
        position += Vector3.up * bounds.extents.magnitude;  // Move up a little

        screenshotCamera.transform.position = position;
        screenshotCamera.transform.LookAt(bounds.center);
        position += Vector3.up * bounds.extents.magnitude * heightOffsetMultiplier;
        screenshotCamera.transform.position = position;


        StartCoroutine(CaptureScreenshot(_presetName, _onDone));
    }

    private IEnumerator CaptureScreenshot(string _presetName, Action _onDone)
    {
        yield return new WaitForEndOfFrame();

        var renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        screenshotCamera.targetTexture = renderTexture;

        var previousActiveTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        screenshotCamera.Render();
        ScreenshotTexture.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        ScreenshotTexture.Apply();

        RenderTexture.active = previousActiveTexture;
        screenshotCamera.targetTexture = null;

        var bytes = ScreenshotTexture.EncodeToPNG();
        var filename = $"{_presetName}.png";
        var path = Path.Combine(FilepathManager.GetApplicationDirectory(), Path.Combine("Screenshots", filename));

        File.WriteAllBytes(path, bytes);
        _onDone?.Invoke();
    }


/*    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private int screenshotWidth = 512;
    [SerializeField] private int screenshotHeight = 512;
    [SerializeField] private string screenshotFilename = "screenshot.png";

    private void Start()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        // Wait for end of frame to ensure everything is rendered
        yield return new WaitForEndOfFrame();

        // Set up the camera based on the gameObject
        // Position the camera at a 45-degree angle to the object and ensure the object is fully visible in the screenshot
*//*        float cameraDistance = screenshotCameraDistance;
        float objectSize = Mathf.Max(objectBounds.size.x, objectBounds.size.y, objectBounds.size.z);
        float requiredDistance = (objectSize / 2f) / Mathf.Tan(screenshotCameraFOV / 2f * Mathf.Deg2Rad);
        if (requiredDistance > cameraDistance)
        {
            cameraDistance = requiredDistance;
        }
        Vector3 cameraPosition = objectBounds.center + screenshotCameraOffset.normalized * cameraDistance;
        screenshotCamera.transform.position = cameraPosition;
        screenshotCamera.transform.LookAt(objectBounds.center);

        // Set the camera's parameters
        screenshotCamera.fieldOfView = screenshotCameraFOV;
        screenshotCamera.aspect = (float)screenshotWidth / screenshotHeight;*//*

        // Set the camera as active to render with it
        screenshotCamera.gameObject.SetActive(true);
        screenshotCamera.targetTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);

        // Create a new texture to save the screenshot to
        Texture2D screenshotTexture = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);

        // Render the screenshot and read the pixels into the texture
        screenshotCamera.Render();
        RenderTexture.active = screenshotCamera.targetTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        screenshotTexture.Apply();

        // Save the texture to a file
        byte[] screenshotBytes = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/" + screenshotFilename, screenshotBytes);

        // Clean up
        screenshotCamera.targetTexture = null;
        screenshotCamera.gameObject.SetActive(false);
        RenderTexture.active = null;

        Debug.Log("Screenshot saved to " + screenshotFilename);
    }*/
}