using Dummiesman;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Importer : MonoBehaviour
{
    public Button importButton;

    private void Start()
    {
        //Button btn = importButton.GetComponent<Button>();
        importButton.onClick.AddListener(ImportOBJModel);
    }

    private void ImportOBJModel()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Import OBJ Model", "", "obj", false);
        if (paths.Length <= 0 || paths[0] == null) return;

        OBJLoader loader = new OBJLoader();
        GameObject importedObj = loader.Load(paths[0]);

        importedObj.transform.position= Vector3.zero;
    }
}
