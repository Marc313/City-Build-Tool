using UnityEngine;

public class Menu : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.isMenuOpen = true;
        }
        else
        {
            FindObjectOfType<UIManager>().isMenuOpen = true;
        }
    }

    protected virtual void OnDisable()
    {
        UIManager.Instance.isMenuOpen = false;
    }
}
