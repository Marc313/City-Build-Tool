using MarcoHelpers;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public Texture2D crosshair;
    public bool isMenuOpen;
    public bool isAllowedOnScreen;

    private void Awake()
    {
        Instance= this;
    }

    private void Start()
    {
        Vector2 cursorOffset = new Vector2(crosshair.width / 4, crosshair.height / 4);
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
    }

    private void OnEnable()
    {
        MarcoHelpers.EventSystem.Subscribe(EventName.MENU_OPENED, OnMenuOpen);
        MarcoHelpers.EventSystem.Subscribe(EventName.MENU_CLOSED, OnMenuClosed);
    }

    private void OnDisable()
    {
        MarcoHelpers.EventSystem.Unsubscribe(EventName.MENU_OPENED, OnMenuOpen);
        MarcoHelpers.EventSystem.Unsubscribe(EventName.MENU_CLOSED, OnMenuClosed);
    }

    private void Update()
    {
        if (isMenuOpen || IsMouseOverUI() || true)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    public static bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnMenuOpen(object _value = null)
    {
        isMenuOpen= true;
    }

    private void OnMenuClosed(object _value = null)
    {
        isMenuOpen= false;
    }
}
