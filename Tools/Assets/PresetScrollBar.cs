using UnityEngine;
using UnityEngine.UI;

public class PresetScrollBar : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] public UIContentFitter contentFitter;

    private void OnEnable()
    {
        if (scrollbar == null) scrollbar = GetComponentInChildren<Scrollbar>();
        if (contentFitter == null) contentFitter= GetComponentInChildren<UIContentFitter>();
    }

    public void ResetScrollValue()
    {
        if (scrollbar == null) return;
        scrollbar.value = 0;
    }

    public void ResizeContentFitter(float _newSize)
    {
        if (contentFitter == null) return;
        contentFitter.Resize(_newSize);
    }
}
