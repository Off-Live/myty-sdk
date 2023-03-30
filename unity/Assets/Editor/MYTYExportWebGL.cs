using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MYTYExportWebGL
    {
        [MenuItem("MYTY SDK/Export/WebGL")]
        private static void ExportWebGL()
        {
            string buildPath = "../react/public/MYTY";

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
            DisableAutoGraphics();

            BuildOptions buildOptions = BuildOptions.None;
            string[] scenes = { UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path };

            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, buildOptions);

            Debug.Log("Exported scene to WebGL at path: " + buildPath);
        }
        
        private static void DisableCompression()
        {
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        }

        private static void DisableStripEngineCode()
        {
            PlayerSettings.stripUnusedMeshComponents = false;
        }

        private static void DisableAutoGraphics()
        {
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
        }
    }
}