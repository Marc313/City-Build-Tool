using SFB;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PresetMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField presetNameField;
    [SerializeField] private TMP_Dropdown categoryDropdown;
    [SerializeField] private TMP_InputField modelPathField;
    [SerializeField] private Button browseButton;
    [SerializeField] private TMP_InputField xSizeField;
    [SerializeField] private TMP_InputField zSizeField;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button cancelButton;

    private EventSystem system;
    private bool isEnabled;
    private Preset.Category currentCategory;

    private void Start()
    {
        SetupSelectable();
        SetupFields();
    }

    private void Update()
    {
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
                Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
                Selectable nextSelectable = current?.FindSelectableOnDown();
                if (nextSelectable != null)
                {
                    nextSelectable.Select();
                }
            }
        }
    }

    public void OpenMenu()
    {
        FindObjectOfType<AnimatedWindow>().MoveDown();
        gameObject.SetActive(true);
    }

    public void Submit()
    {
        Importer importer = FindObjectOfType<Importer>();
        if (importer == null) return;

        string name = presetNameField.text;
        Preset.Category category = currentCategory;
        string objPath = modelPathField.text;
        Vector2 xzScale = new Vector2(float.Parse(xSizeField.text), float.Parse(zSizeField.text));
        importer.ImportOBJModel(name, category, objPath, xzScale);

        gameObject.SetActive(false);

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
        // Submit Button
        submitButton.onClick.AddListener(() => Submit());

        // Browse Button
        browseButton.onClick.AddListener(() => Browse());

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
}
