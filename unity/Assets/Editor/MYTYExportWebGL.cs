using System.Diagnostics;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class MYTYExportWebGL
    {
        public static void ExportWebGL(bool is3D = false)
        {
            string buildPath = "../react/public/WebGL";

            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            {
                if (EditorUtility.DisplayDialog("Change Build Target", "The active build target needs to be changed to WebGL. Do you want to continue?", "Yes", "No"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
                }
                else
                {
                    Debug.LogWarning("Cancelled exporting scene to WebGL.");
                    return;
                }
            }
            
            DisableCompression();
            DisableStripEngineCode();
            AdjustColorSpace();
            if(is3D) AddAlwaysIncludedShaders();

            BuildOptions buildOptions = BuildOptions.None;
            string[] scenes = { SceneManager.GetActiveScene().path };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, buildOptions);
            stopwatch.Stop();
            Debug.Log($"Build Took {stopwatch.ElapsedMilliseconds} ms");

            Debug.Log("Exported scene to WebGL at path: " + buildPath);
        }
        
        private static void DisableCompression()
        {
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        }

        private static void DisableStripEngineCode()
        {
            PlayerSettings.stripEngineCode = false;
        }

        private static void AdjustColorSpace()
        {
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
            PlayerSettings.colorSpace = ColorSpace.Gamma;
        }

        private static void AddAlwaysIncludedShaders()
        {
            var graphicsSettings = AssetDatabase.LoadAssetAtPath<GraphicsSettings>("ProjectSettings/GraphicsSettings.asset");

            Shader shaderToAdd = Shader.Find("Standard");

            if (shaderToAdd == null)
            {
                Debug.LogError("Shader not found. Make sure the shader name is correct.");
                return;
            }

            var serializedObject = new SerializedObject(graphicsSettings);
            var arrayProp = serializedObject.FindProperty("m_AlwaysIncludedShaders");
            var arrayIdx = arrayProp.arraySize;
            arrayProp.InsertArrayElementAtIndex(arrayIdx);
            var arrayElem = arrayProp.GetArrayElementAtIndex(arrayIdx);
            arrayElem.objectReferenceValue = shaderToAdd;

            serializedObject.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();
        }
    }
}