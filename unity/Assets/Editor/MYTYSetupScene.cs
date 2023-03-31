using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MYTYSetupScene
{
    [MenuItem("MYTY SDK/Create 2D Scene")]
    private static void Create2DScene()
    {
        string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTYAvatarObjects.prefab";

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

            string scenePath = "Assets/Scenes/MYTY2D.unity";
            EditorSceneManager.SaveScene(newScene, scenePath);

            EditorSceneManager.SetActiveScene(newScene);

            Selection.activeGameObject = instance;
        }
        else
        {
            Debug.LogError($"Failed to instantiate prefab: {prefabPath}");
        }
    }

    [MenuItem("MYTY SDK/Create 3D Scene")]
    private static void Create3DScene()
    {
        // string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTY3DAvatarObjects.prefab";
        string prefabPath = "Assets/Prefabs/MYTY3DAvatarObjects.prefab";

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

            string scenePath = "Assets/Scenes/MYTY3D.unity";
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
