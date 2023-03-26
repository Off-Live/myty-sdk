using UnityEditor;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(MessageHandler.MessageHandler))]
    public class MessageHandlerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            var tokenIdField = new TextField
            {
                label = "Token ID"
            };
            
            var selectAvatar = new Button
            {
                text = "Select Avatar"
            };
            
            selectAvatar.clicked += () =>
            {
                (target as MessageHandler.MessageHandler)!.SelectAvatar(tokenIdField.value);
            };
            
            var arModeToggle = new Toggle
            {
                label = "AR Mode"
            };
            
            var applyARMode = new Button
            {
                text = "Apply AR Mode"
            };
            
            applyARMode.clicked += () =>
            {
                (target as MessageHandler.MessageHandler)!.SetARMode(arModeToggle.value);
            };

            root.Add(tokenIdField);
            root.Add(selectAvatar);
            
            root.Add(arModeToggle);
            root.Add(applyARMode);
            
            return root;
        }
    }
}