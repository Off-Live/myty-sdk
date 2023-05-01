using AR;
using Motion.MotionProcessor;
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
            var prefabPath = "Assets/Prefabs/Mediapipe.prefab";
            // var prefabPath = "Packages/com.offlive.myty.myty-sdk/Prefabs/Mediapipe.prefab";
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
                
                var motionSource = Object.FindObjectOfType<Motion.MotionSource.MotionSource>();
                var motionProcessor = Object.FindObjectOfType<MotionProcessor>();

                var messageHandler = Object.FindObjectOfType<MessageHandler.MessageHandler>();

                messageHandler.motionSource = motionSource;
                motionSource.motionProcessor = motionProcessor;

                var motionTemplateMapper = Object.FindObjectOfType<MotionTemplateMapper>();
                var arFacePlane = Object.FindObjectOfType<ARFacePlane>();

                if (motionTemplateMapper != null && arFacePlane != null)
                {
                    arFacePlane.motionTemplateMapper = motionTemplateMapper;
                }

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