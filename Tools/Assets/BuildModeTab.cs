using UnityEngine;
using UnityEngine.UI;

public class BuildModeTab : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;

    [SerializeField] private Button buildButton;
    [SerializeField] private Button editButton;
    [SerializeField] private Button demolishButton;

    private Image selectedButtonImage;
    private Builder builder;

    private void Awake()
    {
        builder = FindObjectOfType<Builder>();
    }

    void Start()
    {
        SetupFields();
    }

    private void SetupFields()
    {
        buildButton.onClick.AddListener(BuildButton);
        editButton.onClick.AddListener(EditButton);
        demolishButton.onClick.AddListener(DemolishButton);

        selectedButtonImage = buildButton.GetComponent<Image>();
    }

    private void BuildButton()
    {
        builder.SwitchState(typeof(BuildState));
        SelectButton(buildButton);
    }

    private void EditButton()
    {
        builder.SwitchState(typeof(EditState));
        SelectButton(editButton);
    }

    private void DemolishButton()
    {
        builder.SwitchState(typeof(DemolishState));
        SelectButton(demolishButton);
    }

    public void SetState<T>()
    {
        if (typeof(T) == typeof(BuildState)) SelectButton(buildButton);
        if (typeof(T) == typeof(EditState)) SelectButton(editButton);
        if (typeof(T) == typeof(DemolishState)) SelectButton(demolishButton);
    }

    private void SelectButton(Button _button)
    {
        selectedButtonImage.color = normalColor;
        selectedButtonImage = _button.GetComponent<Image>();
        selectedButtonImage.color = selectedColor;
    }
}
