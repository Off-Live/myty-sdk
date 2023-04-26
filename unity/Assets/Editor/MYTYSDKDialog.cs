using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MYTYSDKDialog : EditorWindow
    {
        private enum Platform
        {
            Web,
            Mobile
        }

        private enum Dimension
        {
            TwoD,
            ThreeD
        }

        private enum MotionCaptureSolution
        {
            Mediapipe,
            ARKit
        }

        private Platform? m_platform;
        private Dimension? m_dimension;
        private MotionCaptureSolution? m_motionCaptureSolution;

        [MenuItem("MYTY SDK/Open Dialog")]
        private static void OpenDialog()
        {
            var window = GetWindow<MYTYSDKDialog>(true, "MYTY SDK");
            window.minSize = new Vector2(300, 200);
            window.maxSize = new Vector2(300, 200);
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Choose Platform", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Toggle(m_platform == Platform.Web, "  Web", EditorStyles.radioButton))
            {
                m_platform = Platform.Web;
            }

            if (GUILayout.Toggle(m_platform == Platform.Mobile, "  Mobile(iOS)", EditorStyles.radioButton))
            {
                m_platform = Platform.Mobile;
            }

            GUILayout.EndHorizontal();

            if (m_platform.HasValue)
            {
                EditorGUILayout.LabelField("Choose 2D or 3D", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                if (GUILayout.Toggle(m_dimension == Dimension.TwoD, "  2D", EditorStyles.radioButton))
                {
                    m_dimension = Dimension.TwoD;
                }

                EditorGUI.BeginDisabledGroup(!PackageManagerWatcher.is3DInstalled);
                if (m_platform == Platform.Web)
                {
                    if (GUILayout.Toggle(m_dimension == Dimension.ThreeD, "  3D", EditorStyles.radioButton))
                    {
                        m_dimension = Dimension.ThreeD;
                    }                
                }
                EditorGUI.EndDisabledGroup();

                GUILayout.EndHorizontal();
            }

            if (m_dimension.HasValue)
            {
                EditorGUILayout.LabelField("Choose Motion Capture Solution", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();

                if (m_platform == Platform.Web && m_dimension == Dimension.TwoD)
                {
                    if (GUILayout.Toggle(m_motionCaptureSolution == MotionCaptureSolution.Mediapipe, "  Mediapipe",
                            EditorStyles.radioButton))
                    {
                        m_motionCaptureSolution = MotionCaptureSolution.Mediapipe;
                    }
                }
                else if (m_platform == Platform.Web && m_dimension == Dimension.ThreeD)
                {
                    if (GUILayout.Toggle(m_motionCaptureSolution == MotionCaptureSolution.Mediapipe, "  Mediapipe",
                            EditorStyles.radioButton))
                    {
                        m_motionCaptureSolution = MotionCaptureSolution.Mediapipe;
                    }
                }
                else if (m_platform == Platform.Mobile)
                {
                    if (GUILayout.Toggle(m_motionCaptureSolution == MotionCaptureSolution.ARKit, "  ARKit",
                            EditorStyles.radioButton))
                    {
                        m_motionCaptureSolution = MotionCaptureSolution.ARKit;
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (m_platform.HasValue && m_dimension.HasValue && m_motionCaptureSolution.HasValue)
            {
                if (GUILayout.Button("Create Scene & Build"))
                {
                    CreateScene();
                    ChooseMotionCaptureSolution();
                    Export();
                    Close();
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("Create Scene"))
                {
                    CreateScene();
                    ChooseMotionCaptureSolution();
                    Close();
                    GUIUtility.ExitGUI();
                }
            }

            GUILayout.EndVertical();
        }
        
        private void CreateScene()
        {
            switch (m_platform)
            {
                case Platform.Web:
                    if (m_dimension == Dimension.TwoD)
                    {
                        MYTYSetupScene.Create2DScene();    
                    }
                    else
                    {
                        MYTYSetupScene.Create3DScene();
                    }
                    break;
                case Platform.Mobile:
                    MYTYSetupScene.CreateMobile2DScene();
                    break;
            }
        }

        private void ChooseMotionCaptureSolution()
        {
            switch (m_motionCaptureSolution)
            {
                case MotionCaptureSolution.Mediapipe:
                    MYTYSelectMotionSource.AddMediapipe();
                    break;
                case MotionCaptureSolution.ARKit:
                    MYTYSelectMotionSource.AddARKit();
                    break;
            }
        }

        private void Export()
        {
            switch (m_platform)
            {
                case Platform.Web:
                    MYTYExportWebGL.ExportWebGL(m_dimension == Dimension.ThreeD);
                    break;
                case Platform.Mobile:
                    MYTYExportIOS.ExportIOS();
                    break;
            }
        }
    }
}