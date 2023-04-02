using MarcoHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotManager : Singleton<ScreenshotManager>
{
    [SerializeField] private Vector3 gameObjectPosition = new Vector3(0, -50.0f, 0);
    [SerializeField] private ThumbnailTaker taker;

    [HideInInspector] public Dictionary<Preset, Sprite> presetSpriteDict = new Dictionary<Preset, Sprite>();

    private void Awake()
    {
        Instance= this;
    }

    private void OnEnable()
    {
        EventSystem.Subscribe(EventName.IMPORT_SUCCESS, AddScreenshot);
    }

    private void OnDisable()
    {
        EventSystem.Unsubscribe(EventName.IMPORT_SUCCESS, AddScreenshot);
    }

    public void ScreenshotAllPresets(List<Preset> _presets)
    {
        FilepathManager.CreateScreenshotDirectory();
        StartCoroutine(nameof(ScreenshotAllPresetsRoutine), _presets);
    }

    public IEnumerator ScreenshotAllPresetsRoutine(List<Preset> _presets)
    {
        foreach (Preset preset in _presets) 
        {
            // If no screenshot
            if (!FilepathManager.ScreenshotExists(preset.presetName))
            {
                StartCoroutine(TakeScreenshot(preset, null));
                yield return new WaitForSeconds(.2f);
            }

            string screenshotPath = FilepathManager.GetScreenshotPath(preset.presetName);
            Sprite screenshotSprite = PngToSprite.ConvertToSprite(taker.screenshotWidth, taker.screenshotHeight, screenshotPath);
            UIManager.Instance.AssignSpriteToButton(preset, screenshotSprite);

            if (screenshotSprite != null 
                && !presetSpriteDict.ContainsKey(preset))
            {
                presetSpriteDict.Add(preset, screenshotSprite);
            }
        }
    }

    public void AddScreenshot(object value = null)
    {
        Preset preset = (Preset)value;
        StartCoroutine(TakeScreenshot(preset, () =>
        {
            string screenshotPath = FilepathManager.GetScreenshotPath(preset.presetName);
            Sprite screenshotSprite = PngToSprite.ConvertToSprite(taker.screenshotWidth, taker.screenshotHeight, screenshotPath);
            UIManager.Instance.AssignSpriteToButton(preset, screenshotSprite);
        }));
    }

    public IEnumerator TakeScreenshot(Preset _preset, Action _onDone = null)
    {
        Debug.Log("Screenshot");
        FindThumbnailTaker();
        if (taker != null)
        {
            GameObject instance = _preset.LoadInstance(gameObjectPosition);
            Debug.Log(instance.gameObject.name + " screenshotting");
            taker.TakeScreenshot(instance, _preset.presetName, () => 
                {
                    Debug.Log(instance.gameObject.name + " deactivated");
                    instance.SetActive(false);
                    _onDone?.Invoke();
                });
        }
        else
        {
            Debug.Log("Thumbnail Taker could not be found!");
        }

        yield return null;
    }

    public ThumbnailTaker FindThumbnailTaker()
    {
        if (taker != null) return taker;
        return GetComponentInChildren<ThumbnailTaker>();
    }
}
