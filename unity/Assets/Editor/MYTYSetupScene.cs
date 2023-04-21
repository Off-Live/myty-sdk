using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MYTYSetupScene
{
    public static void Create2DScene()
    {
        string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTYAvatarObjects.prefab";

        CreateScene(prefabPath, "MYTY2D.unity");
    }

    public static void Create3DScene()
    {
        string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTY3DAvatarObjects.prefab";

        CreateScene(prefabPath, "MYTY3D.unity");
    }

    public static void CreateMobile2DScene()
    {
        string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTYAvatarObjectsMobile.prefab";

        CreateScene(prefabPath, "MYTY2DMobile.unity");
    }

    private static void CreateScene(string prefabPath, string sceneName)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
            return;
        }

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (instance != null)
        {
            PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            instance.name = prefab.name;

            instance.transform.position = Vector3.zero;

            string scenePath = $"Assets/Scenes/{sceneName}";
            EditorSceneManager.SaveScene(newScene, scenePath);

            SceneManager.SetActiveScene(newScene);

            Selection.activeGameObject = instance;
        }
        else
        {
            Debug.LogError($"Failed to instantiate prefab: {prefabPath}");
        }
    }
}
