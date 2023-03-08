using MarcoHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject quickUI;
    public GameObject buttonPrefab;
    public int elementOffset = 200;

    private Builder builder;
    private UIList list;

    private void Awake()
    {
        Instance = this;
        builder = FindObjectOfType<Builder>();
    }

    private void Start()
    {
        list = new UIList(buttonPrefab, quickUI, elementOffset, Vector3.right);
        Invoke(nameof(OnStart), .1f);   
    }

    private void OnEnable()
    {
        EventSystem.Subscribe(EventName.PRESETS_LOADED, LoadPresetCatalogue);
    }

    private void OnDisable()
    {
        EventSystem.Unsubscribe(EventName.PRESETS_LOADED, LoadPresetCatalogue);
    }

    private void OnStart()
    {
        LoadPresetCatalogue();
    }

    private void LoadPresetCatalogue(object value = null)
    {
        list.Reset();
        foreach (Preset preset in PresetCatalogue.presets)
        {
            AddPresetButton(preset);
        }
    }

    public void AddPresetButton(Preset _preset)
    {
        Button button = list.AddElement().GetComponent<Button>();
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(() => builder.SetCurrentPreset(_preset));
        buttonText.text = _preset.presetName;
    }
}
