using UnityEngine;

[System.Serializable]
public class Preset
{
    public GameObject prefab;
    public Category category = Category.Decoration;
    
    public enum Category
    {
        None = 0,
        Road = 1,
        House = 2,
        Decoration = 3
    }
}
