using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MYTYSDKDialog : EditorWindow
    {
        private enum Platform
        {
            Web
            // Mobile
        }

        private enum Dimension
        {
            TwoD,
            ThreeD
        }

        private enum MotionCaptureTool
        {
            Mediapipe,
            ARKit
        }

        private Platform? m_platform;
        private Dimension? m_dimension;
        private MotionCaptureTool? m_motionCaptureTool;

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

            // if (GUILayout.Toggle(m_platform == Platform.Mobile, "  Mobile", EditorStyles.radioButton))
            // {
                // m_platform = Platform.Mobile;
            // }

            GUILayout.EndHorizontal();

            if (m_platform.HasValue)
            {
                EditorGUILayout.LabelField("Choose 2D or 3D", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                if (GUILayout.Toggle(m_dimension == Dimension.TwoD, "  2D", EditorStyles.radioButton))
                {
                    m_dimension = Dimension.TwoD;
                }

                if (GUILayout.Toggle(m_dimension == Dimension.ThreeD, "  3D", EditorStyles.radioButton))
                {
                    m_dimension = Dimension.ThreeD;
                }

                GUILayout.EndHorizontal();
            }

            if (m_dimension.HasValue)
            {
                EditorGUILayout.LabelField("Choose Motion Capture Tool", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();

                if (m_platform == Platform.Web && m_dimension == Dimension.TwoD)
                {
                    if (GUILayout.Toggle(m_motionCaptureTool == MotionCaptureTool.Mediapipe, "  Mediapipe",
                            EditorStyles.radioButton))
                    {
                        m_motionCaptureTool = MotionCaptureTool.Mediapipe;
                    }
                }
                else if (m_platform == Platform.Web && m_dimension == Dimension.ThreeD)
                {
                    if (GUILayout.Toggle(m_motionCaptureTool == MotionCaptureTool.Mediapipe, "  Mediapipe",
                            EditorStyles.radioButton))
                    {
                        m_motionCaptureTool = MotionCaptureTool.Mediapipe;
                    }
                }
                // else if (m_platform == Platform.Mobile)
                // {
                    // if (GUILayout.Toggle(m_motionCaptureTool == MotionCaptureTool.ARKit, "  ARKit",
                            // EditorStyles.radioButton))
                    // {
                        // m_motionCaptureTool = MotionCaptureTool.ARKit;
                    // }
                // }

                GUILayout.EndHorizontal();
            }

            if (m_platform.HasValue && m_dimension.HasValue && m_motionCaptureTool.HasValue)
            {
                if (GUILayout.Button("Create Scene & Build"))
                {
                    if (m_platform == Platform.Web)
                    {
                        if (m_dimension == Dimension.TwoD)
                        {
                            MYTYSetupScene.Create2DScene();

                            if (m_motionCaptureTool == MotionCaptureTool.Mediapipe)
                            {
                                MYTYSelectMotionSource.AddMediapipe();
                            }
                            // else if (m_motionCaptureTool == MotionCaptureTool.Mocap4Face)
                            // {
                                // MYTYSelectMotionSource.AddMocap4Face();
                            // }
                        }
                        else if (m_dimension == Dimension.ThreeD)
                        {
                            MYTYSetupScene.Create3DScene();
                        }
                        
                        MYTYExportWebGL.ExportWebGL(m_dimension == Dimension.ThreeD);
                        
                        Close();
                    }
                    // else if (m_platform == Platform.Mobile)
                    // {
                        // Debug.Log("Mobile is not supported yet");
                    // }
                }

                if (GUILayout.Button("Create Scene"))
                {
                    if (m_platform == Platform.Web)
                    {
                        if (m_dimension == Dimension.TwoD)
                        {
                            MYTYSetupScene.Create2DScene();

                            if (m_motionCaptureTool == MotionCaptureTool.Mediapipe)
                            {
                                MYTYSelectMotionSource.AddMediapipe();
                            }
                        }
                        else if (m_dimension == Dimension.ThreeD)
                        {
                            MYTYSetupScene.Create3DScene();
                        }
                        
                        Close();
                    }
                    // else if (m_platform == Platform.Mobile)
                    // {
                        // Debug.Log("Mobile is not supported yet");
                    // }
                }
            }

            GUILayout.EndVertical();
        }
    }
}