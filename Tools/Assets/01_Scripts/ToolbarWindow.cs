using UnityEngine;
using UnityEngine.UI;

public class ToolbarWindow : MonoBehaviour
{
    [SerializeField] private Button loadButton;

    private void Start()
    {
        SetupFields();
    }

    private void SetupFields()
    {
        if (loadButton != null)
        loadButton.onClick.AddListener(() => SaveManager.Instance.Load());
    }
}
