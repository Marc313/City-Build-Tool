using SFB;
using UnityEngine;
using UnityEngine.UI;
using Autodesk.Fbx;

// The code in this script is somewhat inspired by suggestions from ChatGPT

// Note: This code is extremely inefficient, as it reconstructs every mesh as an FBXMesh.
// It would be way more performant to reconstruct only one instance of every preset, and reuse that reconstruction multiple times.
public class Exporter : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button exportButton;
    public string fileName = "City.fbx";

    private void Start()
    {
        Button btn = exportButton.GetComponent<Button>();
        btn.onClick.AddListener(() => { 
            UIManager.Instance.EnableLoadingScreen(true); 
            Invoke(nameof(ExportFBXScene), .3f); 
        });
    }

    private void ExportFBXScene()
    {
        // Build the fbx scene file path 
        string fbxFilePath = StandaloneFileBrowser.SaveFilePanel("Export City", "", fileName, "fbx");

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

            AddToFBXScene(fbxManager, fbxScene, rootNode, GameObject.Find("Ground"));
            AddToFBXSceneRecursively(fbxManager, fbxScene, rootNode, FindObjectOfType<Builder>().gameObject);

            // Add the root node to the scene.
            fbxScene.GetRootNode().SetName("City");
            fbxScene.GetRootNode().AddChild(rootNode);

            // Export the scene to the file.
            status = fbxExporter.Export(fbxScene);

            // cleanup
            fbxScene.Destroy();
            fbxExporter.Destroy();

            Debug.Log("Export finished!");
            //UIManager.Instance.EnableLoadingScreen(false);
        }

        UIManager.Instance.EnableLoadingScreen(false);
    }

    private void AddToFBXSceneRecursively(FbxManager fbxManager, FbxScene fbxScene, FbxNode rootNode, GameObject obj)
    {
        AddToFBXScene(fbxManager, fbxScene, rootNode, obj);

        // Repeat this function for all children
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            AddToFBXSceneRecursively(fbxManager, fbxScene, rootNode, child.gameObject);
        }
    }

    private void AddToFBXScene(FbxManager fbxManager, FbxScene fbxScene, FbxNode rootNode, GameObject obj)
    {
        MeshFilter filter = obj.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (filter != null)
        {
            if (filter.mesh.subMeshCount == 0 || true)
            {
                AddMesh(fbxManager, fbxScene, rootNode, obj, filter.mesh);
            }
            else
            {
                foreach (Mesh mesh in filter.mesh.SplitSubmeshes(meshRenderer.materials, meshRenderer))
                {
                    AddMesh(fbxManager, fbxScene, rootNode, obj, mesh);
                }
            }
        }
    }

    private void AddMesh(FbxManager fbxManager, FbxScene fbxScene, FbxNode rootNode, GameObject obj, Mesh mesh)
    {
        // Create an FbxNode for the game object.
        FbxNode objNode = FbxNode.Create(fbxScene, obj.name);

        // Create an FbxMesh object
        FbxMesh fbxMesh = FbxMesh.Create(fbxManager, obj.name);

        // Copies all of the data of the mesh
        fbxMesh = CopyMeshData(fbxMesh, mesh);

        if (fbxMesh != null)
        {
            // Set the FbxMesh as the node's geometry.
            objNode.SetNodeAttribute(fbxMesh);

            // Apply the correct transform to _objNode.
            ApplyTransform(obj, objNode);

            // Add the object node as a child of the root node.
            rootNode.AddChild(objNode);
        }

        ApplyMaterials(fbxManager, obj, objNode, fbxMesh);
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

    private void ApplyMaterials(FbxManager _fbxManager, GameObject _obj, FbxNode _objNode, FbxMesh _fbxMesh)
    {
        MeshRenderer renderer = _obj.GetComponent<MeshRenderer>();
        int materialCount = renderer.materials.Length;
        FbxSurfacePhong[] fbxSurfacePhongs = new FbxSurfacePhong[materialCount];

        // Add all materials to the fbxNode
        for (int i = 0; i < materialCount; i++)
        {
            Material material = renderer.materials[i];
            FbxSurfacePhong fbxMaterial = FbxSurfacePhong.Create(_fbxManager, material.name);
            fbxMaterial.SetName(material.name);

            // Copy possible properties over \\

            // Diffuse & Transparency
            fbxMaterial.Diffuse.Set(new FbxDouble3(material.color.r, material.color.g, material.color.b));
            fbxMaterial.TransparencyFactor.Set(material.color.a);

            // Specular Highlights
            if (material.HasFloat("_SpecularHighlights")) 
                fbxMaterial.SpecularFactor.Set(material.GetFloat("_SpecularHighlights"));

            // Shininess
            if (material.HasFloat("_Shininess"))
                fbxMaterial.Shininess.Set(material.GetFloat("_Shininess"));
            
            // Reflection
            if (material.HasFloat("_ReflectAmount"))
                fbxMaterial.ReflectionFactor.Set(material.GetFloat("_ReflectAmount"));
            
            // Set the specular color
            if (material.HasColor("_SpecColor"))
            {
                Color specularColor = material.GetColor("_SpecColor");
                fbxMaterial.Specular.Set(new FbxDouble3(specularColor.r, specularColor.g, specularColor.b));
            }

            // Set the emmisive color
            if (material.HasColor("_EmissionColor"))
            {
                Color emmisiveColor = material.GetColor("_EmissionColor");
                fbxMaterial.Emissive.Set(new FbxDouble3(emmisiveColor.r, emmisiveColor.g, emmisiveColor.b));
            }

            fbxSurfacePhongs[i] = fbxMaterial;
            _objNode.AddMaterial(fbxMaterial);
        }
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