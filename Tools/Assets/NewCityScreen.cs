using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewCityScreen : MonoBehaviour
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
        FilepathManager.projectName = name;
        FilepathManager.CreateUserModelDirectory();
        SaveManager.Instance.Save();
        //string folderPath = folderField.text;

        // Crazy Folder stuff.
        // First Save? Dan kan je die openen. Save folderpath naar save file.
        startScreen.CloseMenu();
    }
}
