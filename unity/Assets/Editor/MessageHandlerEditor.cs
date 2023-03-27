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
            
            var load3DAvatar = new Button
            {
                text = "Load 3D Avatar"
            };
            
            load3DAvatar.clicked += () =>
            {
                (target as MessageHandler.MessageHandler)!.Load3DAvatar();
            };
                
            // var avatarLoader = EditorGUILayout.ObjectField("Avatar Loader", (target as MessageHandler.MessageHandler)!.avatarLoader,typeof(AvatarLoader), true);
            //
            // var avatar3DLoader = EditorGUILayout.ObjectField("Avatar 3D Loader", (target as MessageHandler.MessageHandler)!.avatar3DLoader,typeof(Avatar3DLoader), true);

            root.Add(tokenIdField);
            root.Add(selectAvatar);
            
            root.Add(arModeToggle);
            root.Add(applyARMode);
            
            root.Add(load3DAvatar);
            
            return root;
        }
    }
}