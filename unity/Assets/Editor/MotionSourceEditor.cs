using Motion.MotionSource;
using MotionSource;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Motion.MotionSource.MotionSource), true)]
    public class MotionSourceEditor:UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var categoryField = new PropertyField();
            var templateBridgeField = new PropertyField();
            var motionProcessorField = new PropertyField();
            var categoryButton = new Button();
            var bridgeButton = new Button();
        
            categoryField.BindProperty(serializedObject.FindProperty("motionCategories"));
            templateBridgeField.BindProperty(serializedObject.FindProperty("templateBridgeMap"));
            motionProcessorField.BindProperty(serializedObject.FindProperty("motionProcessor"));
            categoryButton.text = "Autobuild Category";
            categoryButton.clicked += () =>
            {
                var motionSource = target as Motion.MotionSource.MotionSource;
                motionSource.SetupMotionCategory();
                if (!Application.isEditor) return;
            
                var prop = serializedObject.FindProperty("motionCategories");
                var categories = motionSource.GetCategoryList();

                prop.arraySize = categories.Count;
                for (var i = 0; i < categories.Count; i++)
                {
                    var bridges = motionSource.GetBridgesInCategory(categories[i]);
                    var bridgesProp = prop.GetArrayElementAtIndex(i).FindPropertyRelative("bridges");
                    prop.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue = categories[i];
                    bridgesProp.arraySize = bridges.Count;
                    for (var j = 0; j < bridges.Count; j++)
                    {
                        bridgesProp.GetArrayElementAtIndex(j).objectReferenceValue = bridges[j];
                    }
                }

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(motionSource);
         
            };

            bridgeButton.text = "Autofill bridge name";
            bridgeButton.clicked += () =>
            {
                var motionSource = target as Motion.MotionSource.MotionSource;
                SetupBridge(motionSource);
            };
        
            root.Add(categoryField);
            root.Add(templateBridgeField);
            root.Add(motionProcessorField);
            root.Add(categoryButton);
            root.Add(bridgeButton);
            return root;
        }
    
        void SetupBridge(Motion.MotionSource.MotionSource source)
        {   
            var bridgeProp = serializedObject.FindProperty("templateBridgeMap");
            var mt = source.motionProcessor.motionTemplateMapperList;

            var names = mt[0].GetNames();
            bridgeProp.arraySize = names.Count;
            for (int i = 0; i < bridgeProp.arraySize; i++)
            {
                bridgeProp.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue = names[i];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}