using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(MessageHandler.MessageHandler))]
    public class MessageHandlerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            var assetVersionIdField1 = new LongField
            {
                label = "Asset Version ID"
            };
            
            var templateAssetUriField = new TextField
            {
                label = "Template Uri"
            };
            
            var tokenIdField1 = new TextField
            {
                label = "Token ID"
            };
            
            var tokenAssetUriField = new TextField
            {
                label = "Token Uri"
            };
            
            var loadAvatarButton = new Button
            {
                text = "Load Avatar"
            };
            
            loadAvatarButton.clicked += (() =>
            {
                (target as MessageHandler.MessageHandler)!.LoadAvatar(
                    assetVersionIdField1.value,
                    templateAssetUriField.value,
                    tokenIdField1.value,
                    tokenAssetUriField.value
                    );
            });
            
            var assetVersionIdField2 = new LongField
            {
                label = "Asset Version ID"
            };
            
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
                (target as MessageHandler.MessageHandler)!.SelectAvatar(assetVersionIdField2.value, tokenIdField.value);
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

            root.Add(assetVersionIdField1);
            root.Add(tokenIdField1);
            root.Add(templateAssetUriField);
            root.Add(tokenAssetUriField);
            root.Add(loadAvatarButton);
            
            root.Add(assetVersionIdField2);
            root.Add(tokenIdField);
            root.Add(selectAvatar);
            
            root.Add(arModeToggle);
            root.Add(applyARMode);
            
            root.Add(load3DAvatar);
            
            return root;
        }
    }
}