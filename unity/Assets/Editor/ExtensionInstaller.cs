using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Editor
{
    public class ExtensionInstaller
    {
        static AddAndRemoveRequest m_request;
        static readonly string[] MytySDK3DPackages =
        {
            "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.109.0",
            "https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.109.0",
            "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.109.0",
            "https://github.com/Off-Live/myty-3d-avatar-extension.git?path=/Assets",
            "https://github.com/Off-Live/myty-sdk-3d-extension.git?path=/unity/Assets#pre-release/dev"
        };
        
        [MenuItem("MYTY SDK/Install Extensions/3D Extension", false, 200)]
        static void InstallMYTY3D()
        {
            m_request = Client.AddAndRemove(MytySDK3DPackages, null);
            EditorUtility.DisplayProgressBar("MYTY SDK","Installing packages",0.5f);
            EditorApplication.update += Progress;
        }
    
        static void Progress()
        {
            if (m_request.IsCompleted)
            {
                if (m_request.Status == StatusCode.Success)
                {
                    Debug.Log("3D Installation Done!");
                    PackageManagerWatcher.is3DInstalled = true;
                    EditorApplication.update -= Progress;
                    EditorUtility.ClearProgressBar();
                }
                else if (m_request.Status >= StatusCode.Failure)
                {
                    Debug.LogError(m_request.Error.message);
                    EditorApplication.update -= Progress;
                    EditorUtility.ClearProgressBar();
                }
            
            }
        }
    }
}