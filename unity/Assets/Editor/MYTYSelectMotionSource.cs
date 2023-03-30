using AR;
using Avatar;
using MYTYKit.MotionTemplates;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class MYTYSelectMotionSource
    {
        [MenuItem("MYTY SDK/Select MotionSource/Mediapipe")]
        private static void AddMediapipe()
        {
            var prefabPath = "Assets/Prefabs/Mediapipe.prefab";
            
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
            }
            else
            {
                Debug.LogError("Failed to instantiate prefab: " + prefabPath);
            }
        }
    }
}