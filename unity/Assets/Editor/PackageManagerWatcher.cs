using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Editor
{
    public class PackageManagerWatcher
    {
        public static bool is3DInstalled = false;
        private static ListRequest m_listReq;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            m_listReq = Client.List();
            EditorApplication.update += CheckIfInstalled;
        }

        private static void CheckIfInstalled()
        {
            if (m_listReq.IsCompleted)
            {
                if (m_listReq.Status == StatusCode.Success)
                {
                    if (m_listReq.Result.Any(package => package.name == "com.offlive.myty.myty-sdk.3d"))
                    {
                        is3DInstalled = true;
                    }
                }
                else
                {
                    Debug.Log($"Could not check for packages: {m_listReq.Error.message}");
                }
                EditorApplication.update -= CheckIfInstalled;
            }
        }
    }
}