using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCityScreen : Menu
{
    [SerializeField] private Button createButton;
    [SerializeField] private StartScreen startScreen;

    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField folderField;

    private void Start()
    {
        SetupFields();
    }

    public void InjectStartScreen(StartScreen _startScreen)
    {
        startScreen = _startScreen;
    }

    private void SetupFields()
    {
        createButton.onClick.AddListener(CreateCity);
    }

    private void CreateCity()
    {
        string name = nameField.text;
        SaveSystem.fileName = name;
        FilepathManager.projectName = name;
        FilepathManager.CreateUserModelDirectory();
        SaveManager.Instance.Save(true);
        //string folderPath = folderField.text;

        // Crazy Folder stuff.
        // First SaveAs? Dan kan je die openen. SaveAs folderpath naar save file.
        startScreen.CloseMenu();
    }
}
