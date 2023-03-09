using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup;
    public Preset.Category category;
    [HideInInspector] public Image backgroundImage;

    private Button button;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        button = GetComponent<Button>();
        tabGroup.Add(this);
    }

    private void Start()
    {
        button.onClick.AddListener(() => tabGroup.OnTabSelected(this));
    }
}
