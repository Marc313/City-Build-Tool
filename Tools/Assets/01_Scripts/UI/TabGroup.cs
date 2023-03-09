using JetBrains.Annotations;
using MarcoHelpers;
using System.Collections.Generic;
using UnityEngine;

// Manager for a group of tabs
public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;

    [SerializeField] private Color NormalColor;
    [SerializeField] private Color onSelectedColor;
    private TabButton currentButton;

    private void Start()
    {
        Debug.Log(tabButtons);
        if (tabButtons != null && tabButtons.Count > 0)
        {
            OnTabSelected(tabButtons[0]);
        }
    }

    public void Add(TabButton _tabButton)
    {
        if (tabButtons == null) tabButtons = new List<TabButton>();
        tabButtons.Add(_tabButton);
    }

    public void OnTabExit(TabButton _button)
    {
        _button.backgroundImage.color = NormalColor;
    }

    public void OnTabSelected(TabButton _button)
    {
        if (currentButton != null) OnTabExit(currentButton);
        currentButton= _button;
        _button.backgroundImage.color = onSelectedColor;
        EventSystem.RaiseEvent(EventName.TAB_CHANGED, _button.category);
    }
}
