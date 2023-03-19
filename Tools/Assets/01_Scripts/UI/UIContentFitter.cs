using Ookii.Dialogs;
using UnityEngine;

public class UIContentFitter : MonoBehaviour
{
    private float startWidth;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        startWidth = rectTransform.rect.width;   
    }

    public void Resize(float _newWidth)
    {
        if (rectTransform == null) return;
        rectTransform.sizeDelta = new Vector2(Mathf.Clamp(_newWidth, startWidth, float.MaxValue), rectTransform.sizeDelta.y);
    }
}
