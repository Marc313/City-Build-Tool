using MarcoHelpers;
using SFB;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventSystem = UnityEngine.EventSystems.EventSystem;

public class PresetMenu : Menu
{
    [SerializeField] private TMP_InputField presetNameField;
    [SerializeField] private TMP_Dropdown categoryDropdown;
    [SerializeField] private TMP_InputField modelPathField;
    [SerializeField] private Button browseButton;
    [SerializeField] private TMP_InputField xSizeField;
    [SerializeField] private TMP_InputField zSizeField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameObject errorMenu;

    private List<TMP_InputField> inputFields = new List<TMP_InputField>();
    private EventSystem system;
    private bool isEnabled;
    private Preset.Category currentCategory;

    private void Start()
    {
        SetupSelectable();
        SetupFields();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MarcoHelpers.EventSystem.Subscribe(EventName.ON_OBJNAME_ALREADY_EXISTS, ShowDuplicationError);
        MarcoHelpers.EventSystem.Subscribe(EventName.IMPORT_SUCCESS, OnImportSucces);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MarcoHelpers.EventSystem.Unsubscribe(EventName.ON_OBJNAME_ALREADY_EXISTS, ShowDuplicationError);
        MarcoHelpers.EventSystem.Unsubscribe(EventName.IMPORT_SUCCESS, OnImportSucces);
    }

    private void Update()
    {
        //CheckEnableInput();

        if (!isEnabled) return;
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
                Selectable previousSelectable = current?.FindSelectableOnUp();
                if (previousSelectable != null)
                {
                    previousSelectable.Select();
                }
            }
            else
            {
                Selectable current = system.currentSelectedGameObject?.GetComponent<Selectable>();
                Selectable nextSelectable = current?.FindSelectableOnDown();
                if (nextSelectable != null)
                {
                    nextSelectable.Select();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

/*    private void CheckEnableInput()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            OpenMenu();
        }
    }*/

    public void OpenMenu()
    {
        MarcoHelpers.EventSystem.RaiseEvent(EventName.MENU_OPENED);
        isEnabled = true;
        gameObject.SetActive(true);
        FindObjectOfType<AnimatedWindow>().MoveDown();
    }

    public void CloseMenu()
    {
        MarcoHelpers.EventSystem.RaiseEvent(EventName.MENU_CLOSED);
        isEnabled = false;
        ResetInputFields();
        errorMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Submit()
    {
        Importer importer = FindObjectOfType<Importer>();
        if (importer == null) return;

        string name = presetNameField.text;
        Preset.Category category = (Preset.Category) categoryDropdown.value + 1;
        string objPath = modelPathField.text;
        Vector2 xzScale = new Vector2(float.Parse(xSizeField.text), float.Parse(zSizeField.text));
        importer.ImportOBJModel(name, category, objPath, xzScale);
    }

    private void ResetInputFields()
    {
        categoryDropdown.value = 0;
        foreach (TMP_InputField field in inputFields) field.text = String.Empty;
    }

    public void OnCategoryChanged(int _index)
    {
        currentCategory = (Preset.Category)_index + 1;
    }

    private void Browse()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Import OBJ Model", "", "obj", false);
        if (paths.Length <= 0 || paths[0] == null) return;

        // Copy File to UserModels folder
        string filePath = paths[0];
        modelPathField.text = filePath;
    }

    private void SetupFields()
    {
        // Input Fields
        inputFields = new List<TMP_InputField>()
        {
            presetNameField,
            modelPathField,
            xSizeField,
            zSizeField,
        };

        // Submit Button
        submitButton.onClick.AddListener(() => Submit());

        // Browse Button
        browseButton.onClick.AddListener(() => Browse());

        // Cancel Button
        cancelButton.onClick.AddListener(() => CloseMenu());

        // Dropdown
        categoryDropdown.onValueChanged.AddListener((int i) => OnCategoryChanged(i));
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        for (int i = 1; i < Enum.GetValues(typeof(Preset.Category)).Length; i++)
        {
            Preset.Category currentCategory = (Preset.Category)i;
            optionList.Add(new TMP_Dropdown.OptionData(currentCategory.ToString()));
        }

        categoryDropdown.options = optionList;
    }

    private void SetupSelectable()
    {
        system = EventSystem.current;
        Selectable nameSelectable = presetNameField.GetComponent<Selectable>();

        if (system != null && nameSelectable != null)
        {
            nameSelectable.Select();
        }
        else
        {
            Debug.Log("PresetMenu: First selectable or Eventsystem missing");
        }
    }

    private void ShowDuplicationError(object _value = null)
    {
        errorMenu.SetActive(true);
    }

    private void OnImportSucces(object _value = null)
    {
        Invoke(nameof(CloseMenu), 0.01f);
    }
}
