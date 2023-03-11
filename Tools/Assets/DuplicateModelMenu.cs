using UnityEngine;
using UnityEngine.UI;

public class DuplicateModelMenu : MonoBehaviour
{
    [SerializeField] private Button duplicateButton;
    [SerializeField] private Button replaceButton;
    [SerializeField] private Button cancelButton;

    private Importer importer;

    private void Start()
    {
        importer = FindObjectOfType<Importer>();
        SetupClickEvents();
    }

    private void SetupClickEvents()
    {
        duplicateButton.onClick.AddListener(() => importer.CreateNewWithOtherName());
        replaceButton.onClick.AddListener(() => importer.ReplaceFile());
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
