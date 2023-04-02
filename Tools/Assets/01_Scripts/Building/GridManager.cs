using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float gridStartSize = 1.0f;
    public float startOpacity = 0.18f;
    public float shortcutInterval = 0.25f;

    [SerializeField] private Material gridMaterial;
    [SerializeField] private TMP_InputField sizeField;
    [SerializeField] private TMP_InputField opacityField;

    public void Start()
    {
        gridMaterial.SetFloat("_CellSize", gridStartSize);
        sizeField.text = gridStartSize.ToString();
        opacityField.text = startOpacity.ToString();

        // Subscribe events
        sizeField.onEndEdit.AddListener((string text) => OnSizeFieldDeselect(text));
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Period)) 
        {
            SetCellSize(Grid.cellSize + shortcutInterval);
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Comma))
        {
            SetCellSize(Grid.cellSize - shortcutInterval);
        }
    }

    public void OnSizeFieldDeselect(string _text)
    {
        float value = float.Parse(_text);
        SetCellSize(value);
    }

    public void SetCellSize(float _value)
    {
        Debug.Log("Set Cell Size!!");
        gridMaterial.SetFloat("_CellSize", _value);
        Grid.cellSize = _value;
        sizeField.text = _value.ToString();
    }

    public void SetOpacity(float _value)
    {
        Debug.Log("Spannend");
    }
}
