using UnityEngine;

[CreateAssetMenu(menuName = "Functions/Menu")]
public class MenuFunctions : ScriptableObject
{
    public void Quit()
    {
        SaveManager.Instance.Save(false);
        Application.Quit();
        Debug.Log("Quit");
    }
}
