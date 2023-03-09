using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D crosshair;

    private void Start()
    {
        Vector2 cursorOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
    }

    private void Update()
    {
        if (IsMouseOverUI())
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
        return EventSystem.current.IsPointerOverGameObject();
    }
}
