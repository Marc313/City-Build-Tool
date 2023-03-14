using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button newCityButton;
    [SerializeField] private Button openCityButton;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject newCityMenu;

    private void Awake()
    {
        NewCityScreen newCityScreen = newCityMenu.GetComponent<NewCityScreen>();
        if (newCityScreen != null) newCityScreen.InjectStartScreen(this);
    }

    private void Start()
    {
        startMenu.SetActive(true);
        SetupFields();
    }

    public void CloseMenu()
    {
        UIManager.Instance.ShowBuilderUI();     // Set other menu's active.
        newCityMenu.SetActive(false);
        startMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SetupFields()
    {
        newCityButton.onClick.AddListener(NewCity);
        openCityButton.onClick.AddListener(Open);
    }

    private void Open()
    {
        bool loadedCity = SaveManager.Instance.Load();
        if (loadedCity) CloseMenu();
    }

    private void NewCity()
    {
        newCityMenu.SetActive(true);
        startMenu.SetActive(false);
    }

}
