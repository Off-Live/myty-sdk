using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class MYTYExportIOS
    {
        public static void ExportIOS()
        {
            string buildPath = EditorUtility.OpenFolderPanel("Choose Build Path", ".", "") + "/UnityLibrary";
            
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            
            if (Directory.Exists(buildPath))
                Directory.Delete(buildPath, true);
            
            EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;
            
            PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS, Il2CppCompilerConfiguration.Release);
            EditorUserBuildSettings.il2CppCodeGeneration = UnityEditor.Build.Il2CppCodeGeneration.OptimizeSize;

            string[] scenes = {SceneManager.GetActiveScene().path};
            var playerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                target = BuildTarget.iOS,
                locationPathName = buildPath
            };

            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result != BuildResult.Succeeded)
                throw new Exception("Build failed");

#if UNITY_IOS
            XcodePostBuild.PostBuild(BuildTarget.iOS, report.summary.outputPath);
#endif
        }
    }
}