using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MYTYSetupScene
{
    [MenuItem("MYTY SDK/Create 2D Scene")]
    private static void CreateScene()
    {
        string prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MYTYAvatarObjects.prefab";
        string messageHandlerPrefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/MessageHandler.prefab";

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        GameObject messageHandlerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(messageHandlerPrefabPath);

        if (prefab == null || messageHandlerPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath} / {messageHandlerPrefabPath}");
            return;
        }

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        GameObject messageHandler = PrefabUtility.InstantiatePrefab(messageHandlerPrefab) as GameObject;

        if (instance != null && messageHandler != null)
        {
            instance.name = prefab.name;
            messageHandler.name = messageHandlerPrefab.name;

            instance.transform.position = Vector3.zero;
            messageHandler.transform.position = Vector3.zero;

            string scenePath = "Assets/Scenes/MYTY2D.unity";
            EditorSceneManager.SaveScene(newScene, scenePath);

            EditorSceneManager.SetActiveScene(newScene);

            Selection.activeGameObject = instance;
        }
        else
        {
            Debug.LogError($"Failed to instantiate prefab: {prefabPath} / {messageHandlerPrefabPath}");
        }
    }
}
