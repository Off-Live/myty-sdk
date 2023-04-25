using AR;
using Avatar.Interface;
using MYTYKit.MotionTemplates;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class MYTYSelectMotionSource
    {
        public static void AddMediapipe()
        {
            var prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/Mediapipe.prefab";
            AddMotionSource(prefabPath);
        }

        public static void AddARKit()
        {
            var prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/ARKit.prefab";
            AddMotionSource(prefabPath);
        }

        private static void AddMotionSource(string prefabPath)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (prefab == null)
            {
                Debug.LogError("Prefab not found at path: " + prefabPath);
                return;
            }
            
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            
            if (instance != null)
            {
                instance.name = prefab.name;
                PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                instance.transform.position = Vector3.zero;
                
                SceneManager.MoveGameObjectToScene(instance, SceneManager.GetActiveScene());
                
                var motionSource = Object.FindObjectOfType<MotionSource.MotionSource>();
                
                var avatarManager = Object.FindObjectOfType<AvatarManager>();
                var messageHandler = Object.FindObjectOfType<MessageHandler.MessageHandler>();

                messageHandler.motionSource = motionSource;
                avatarManager.motionSource = motionSource;
                
                var motionTemplateMapper = Object.FindObjectOfType<MotionTemplateMapper>();
                
                var arFacePlane = Object.FindObjectOfType<ARFacePlane>();
                
                arFacePlane.motionTemplateMapper = motionTemplateMapper;

                Selection.activeGameObject = instance;
                
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), SceneManager.GetActiveScene().path);
            }
            else
            {
                Debug.LogError("Failed to instantiate prefab: " + prefabPath);
            }
        }
    }
}