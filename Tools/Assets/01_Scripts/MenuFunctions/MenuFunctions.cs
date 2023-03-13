using UnityEngine;

[CreateAssetMenu(menuName = "Functions/Menu")]
public class MenuFunctions : ScriptableObject
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
