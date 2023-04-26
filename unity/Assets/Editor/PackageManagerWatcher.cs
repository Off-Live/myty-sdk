using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;

namespace Editor
{
    public class PackageManagerWatcher
    {
        public static bool is3DInstalled = false;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update += CheckIfInstalled;
        }

        private static void CheckIfInstalled()
        {
            var request = Client.List();
            while (!request.IsCompleted)
            {
            }

            if (request.Status == StatusCode.Success)
            {
                if (request.Result.Any(package => package.name == "com.offlive.myty.myty-sdk.3d"))
                {
                    is3DInstalled = true;
                    EditorApplication.update -= CheckIfInstalled;
                }
            }
        }
    }
}