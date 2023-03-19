using MarcoHelpers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [HideInInspector] public bool isMenuOpen;

    [Header("Preset Menu")]
    [SerializeField] private GameObject presetMenu;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private PresetScrollBar presetScrollBar;
    [SerializeField] private Transform uiListStartPos;
    [SerializeField] private int elementOffset = 200;

    [Header("Builder Menus")]
    [SerializeField] private AnimatedWindow presetWindow;
    [SerializeField] private GameObject quickUI;
    [SerializeField] private GameObject buildModeTab;

    [Header("Quick Screens")]
    [SerializeField] private GameObject loadingScreen;

    [Header("Debug Info")]
    [SerializeField] private TMP_Text logOutput;
    [SerializeField] private float logDuration;

    private Builder builder;
    private Dictionary<Preset.Category, UIList> uiLists;
    private UIList currentlyVisiblePresetList;

    //private UIList list;

    private void Awake()
    {
        Instance = this;
        builder = FindObjectOfType<Builder>();
    }

    private void OnEnable()
    {
        EventSystem.Subscribe(EventName.PRESETS_LOADED, LoadPresetCatalogue);
        EventSystem.Subscribe(EventName.TAB_CHANGED, OnTabChanged);
    }

    private void OnDisable()
    {
        EventSystem.Unsubscribe(EventName.PRESETS_LOADED, LoadPresetCatalogue);
        EventSystem.Unsubscribe(EventName.TAB_CHANGED, OnTabChanged);
    }

    private void OnStart()
    {
        LoadPresetCatalogue();
    }

    public void AddPresetButton(Preset _preset)
    {
        if (uiLists == null)
        {
            Logger.Log("Preset Lists could not be found!");
            LoadPresetCatalogue();
        }

        UIList categoryList = uiLists[_preset.category];
        Button button = categoryList.AddElement().GetComponent<Button>();
        button.onClick.AddListener(() => builder.SetCurrentPreset(_preset));
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        buttonText.text = _preset.presetName;
        presetScrollBar.ResizeContentFitter(categoryList.GetTotalSize());
    }

    public void ShowBuilderUI()
    {
        presetMenu.SetActive(true);
        presetWindow.MoveUp();
        quickUI.SetActive(true);
        buildModeTab.SetActive(true);
    }

    public void EnableLoadingScreen(bool _isEnabled)
    {
        loadingScreen.SetActive(_isEnabled);
    }

    // Leave this to logger class
    public void ShowLogText(string _text)
    {
        logOutput.gameObject.SetActive(true);
        logOutput.text += _text + "\n";
        CancelInvoke(nameof(HideLogText));
        Invoke(nameof(HideLogText), logDuration);
    }

    private void HideLogText()
    {
        logOutput.gameObject.SetActive(false);
        logOutput.text = string.Empty;
    }

    private void LoadPresetCatalogue(object value = null)
    {
        uiLists = new Dictionary<Preset.Category, UIList>();

        for (int i = 1; i < Enum.GetValues(typeof(Preset.Category)).Length; i++)
        {
            Preset.Category category = (Preset.Category)i;
            string buttonParentName = $"{category.ToString().ToLower()}Buttons";
            GameObject buttonParent = CreateButtonParent(buttonParentName);
            uiLists[category] = new UIList(buttonPrefab, buttonParent, elementOffset, Vector3.right);
        }

        foreach (Preset preset in PresetCatalogue.allPresets)
        {
            AddPresetButton(preset);
        }
    }

    private GameObject CreateButtonParent(string buttonParentName)
    {
        GameObject buttonParent = new GameObject(buttonParentName);
        buttonParent.transform.position = uiListStartPos.position;
        buttonParent.transform.parent = presetMenu.transform;
        buttonParent.SetActive(false);
        return buttonParent;
    }


    private void OnTabChanged(object _value = null)
    {
        if (uiLists == null) return;

        Preset.Category category = (Preset.Category) _value;
        UIList categoryList = uiLists[category];
        presetScrollBar.ResetScrollValue();
        if (currentlyVisiblePresetList!= null)
        {
            currentlyVisiblePresetList.GetParent().transform.parent = presetMenu.transform;
            currentlyVisiblePresetList?.EnableParent(false);
        }
        categoryList?.EnableParent(true);
        categoryList.GetParent().transform.parent = presetScrollBar.contentFitter.transform;
        presetScrollBar.ResizeContentFitter(categoryList.GetTotalSize());

        currentlyVisiblePresetList = categoryList;
    }
}
