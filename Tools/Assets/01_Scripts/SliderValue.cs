using TMPro;
using UnityEngine;

public class SliderValue : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateValue(float _value)
    {
        text.text = _value.ToString();
    }
}
