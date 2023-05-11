using AR;
using Motion.ARKit;
using Motion.Mediapipe;
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
                
                var motionSource = Object.FindObjectOfType<Motion.MotionSource.MotionSource>();
                var motionProcessor = Object.FindObjectOfType<MotionProcessor>();

                var messageHandler = Object.FindObjectOfType<MessageHandler.MessageHandler>();
                var arFaceControl = Object.FindObjectOfType<ARFaceControl>();

                messageHandler.motionSource = motionSource;
                if (arFaceControl != null)
                {
                    arFaceControl.motionSource = motionSource;
                }
                motionSource.motionProcessor = motionProcessor;

                if (prefabPath.Contains("Mediapipe"))
                {
                    (motionSource as MPMotionSource)!.arBounds = GameObject.FindWithTag("MainRenderer")?.GetComponent<MeshRenderer>();
                } else if (prefabPath.Contains("ARKit"))
                {
                    (motionSource as ARKitMotionSource)!.mainCamera = Camera.main;
                    (motionSource as ARKitMotionSource)!.renderingObjects = GameObject.FindWithTag("MainRenderer").transform.parent.gameObject;
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