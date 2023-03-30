using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MYTYSetupScene
{
    [MenuItem("MYTY SDK/Create 2D Scene")]
    private static void CreateScene()
    {
        string prefabPath = "Assets/Prefabs/MYTYAvatarObjects.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError("Prefab not found at path: " + prefabPath);
            return;
        }

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (instance != null)
        {
            instance.name = prefab.name;

            instance.transform.position = Vector3.zero;

            string scenePath = "Assets/Scenes/MYTY2D.unity";
            EditorSceneManager.SaveScene(newScene, scenePath);

            EditorSceneManager.SetActiveScene(newScene);

            Selection.activeGameObject = instance;
        }
        else
        {
            Debug.LogError("Failed to instantiate prefab: " + prefabPath);
        }
    }
}
