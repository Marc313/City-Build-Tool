using SFB;
using UnityEngine;
using UnityEngine.UI;
using Autodesk.Fbx;
using UnityEngine.SceneManagement;

public class Exporter : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button exportButton;
    public string fileName = "DemoScene.fbx";

    private void Start()
    {
        Button btn = exportButton.GetComponent<Button>();
        btn.onClick.AddListener(ExportFBXScene);
    }

    private void ExportFBXScene()
    {
        // Build the fbx scene file path 
        string fbxFilePath = StandaloneFileBrowser.SaveFilePanel("Export City", "", "city.fbx", "fbx");
        Debug.Log(string.Format("The file that will be written is {0}", fbxFilePath));

        using (var fbxManager = FbxManager.Create())
        {
            // Configure the IO settings.
            FbxIOSettings fbxIOSettings = FbxIOSettings.Create(fbxManager, Globals.IOSROOT);
            fbxManager.SetIOSettings(fbxIOSettings);

            // Create the exporter 
            var fbxExporter = FbxExporter.Create(fbxManager, "Exporter");

            // Initialize the exporter. "FBX binary (*.fbx)" is also supported by Blender ;)
            int fileFormat = fbxManager.GetIOPluginRegistry().FindWriterIDByDescription("FBX binary (*.fbx)");

            bool status = fbxExporter.Initialize(fbxFilePath, fileFormat, fbxIOSettings);
            // Check that initialization of the fbxExporter was successful
            if (!status)
            {
                Debug.LogError(string.Format("failed to initialize exporter, reason: " +
                                               fbxExporter.GetStatus().GetErrorString()));
                return;
            }

            // Create a scene
            var fbxScene = FbxScene.Create(fbxManager, "Building Scene");

            SetDocumentInfo(fbxManager, fbxScene);

            // Create an FbxNode for the root of the scene.
            FbxNode rootNode = FbxNode.Create(fbxScene, "RootNode");

            // Iterate through all the game objects in the scene.
            foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                AddToFBXScene(fbxManager, fbxScene, rootNode, obj);
            }

            // Add the root node to the scene.
            fbxScene.GetRootNode().SetName("City");
            fbxScene.GetRootNode().AddChild(rootNode);

            // Export the scene to the file.
            status = fbxExporter.Export(fbxScene);

            // cleanup
            fbxScene.Destroy();
            fbxExporter.Destroy();

            Debug.Log("Export without errors");
            //ModelExporter.ExportObjects(fbxFilePath, SceneManager.GetActiveScene().GetRootGameObjects());
        }
    }

    private void AddToFBXScene(FbxManager fbxManager, FbxScene fbxScene, FbxNode rootNode, GameObject obj)
    {
        MeshFilter filter = obj.GetComponent<MeshFilter>();
        if (filter != null)
        {
            // Create an FbxNode for the game object.
            FbxNode objNode = FbxNode.Create(fbxScene, obj.name);

            FbxMesh fbxMesh = FbxMesh.Create(fbxManager, obj.name);
            // Create an FbxMesh object

            fbxMesh = CopyMeshData(fbxMesh, filter.mesh);

            if (fbxMesh != null)
            {
                // Set the FbxMesh as the node's geometry.
                objNode.SetNodeAttribute(fbxMesh);

                // Apply the correct transform to _objNode.
                ApplyTransform(obj, objNode);

                // Add the object node as a child of the root node.
                rootNode.AddChild(objNode);
            }
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            AddToFBXScene(fbxManager, fbxScene, rootNode, child.gameObject);
        }
    }

    private static void ApplyTransform(GameObject _obj, FbxNode _objNode)
    {
        Vector3 pos = _obj.transform.position;
        Vector3 rot = _obj.transform.rotation.eulerAngles;
        Vector3 scale = _obj.transform.lossyScale;

        FbxDouble3 translation = new FbxDouble3(pos.x, pos.y, pos.z);
        FbxDouble3 rotation = new FbxDouble3(rot.x, rot.y, rot.z);
        FbxDouble3 scaling = new FbxDouble3(scale.x, scale.y, scale.z);

        _objNode.LclTranslation.Set(translation);
        _objNode.LclRotation.Set(rotation);
        _objNode.LclScaling.Set(scaling);
    }

    private FbxMesh CopyMeshData(FbxMesh _fbxMesh, Mesh _mesh)
    {
        // Set the control points of the _mesh
        int vertexCount = _mesh.vertices.Length;
        int controlPointsCount = vertexCount / 3;
        _fbxMesh.InitControlPoints(controlPointsCount);

        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = _mesh.vertices[i];
            _fbxMesh.SetControlPointAt(new FbxVector4(vertex.x, vertex.y, vertex.z), i);
        }

        // Set the polygon data of the _mesh
        int[] triangles = _mesh.triangles;
        int triangleCount = triangles.Length / 3;

        int polygonIndex = 0;
        for (int i = 0; i < triangleCount; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            _fbxMesh.BeginPolygon(polygonIndex);
            _fbxMesh.AddPolygon(vertexIndex1);
            _fbxMesh.AddPolygon(vertexIndex2);
            _fbxMesh.AddPolygon(vertexIndex3);
            _fbxMesh.EndPolygon();

            polygonIndex++;
        }

        return _fbxMesh;
    }

    private static void SetDocumentInfo(FbxManager fbxManager, FbxScene fbxScene)
    {
        // create scene info
        FbxDocumentInfo fbxSceneInfo = FbxDocumentInfo.Create(fbxManager, "SceneInfo");

        // set some scene info values
        fbxSceneInfo.mTitle = "fromRuntime";
        fbxSceneInfo.mSubject = "Exported from a Unity runtime";
        fbxSceneInfo.mAuthor = "City Builder - Marc Neeleman";
        fbxSceneInfo.mRevision = "1.0";
        fbxSceneInfo.mKeywords = "export runtime";
        fbxSceneInfo.mComment = "This is to demonstrate the capability of exporting from a Unity runtime, using the FBX SDK C# bindings";

        fbxScene.SetSceneInfo(fbxSceneInfo);
    }
}