using Data;
using Newtonsoft.Json;
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
            
            var avatarLoader = new PropertyField();
            avatarLoader.BindProperty(serializedObject.FindProperty("m_avatarDownloader"));

            var avatarManager = new PropertyField();
            avatarManager.BindProperty(serializedObject.FindProperty("m_avatarManager"));
            
            var motionSource = new PropertyField();
            motionSource.BindProperty(serializedObject.FindProperty("motionSource"));
            
            root.Add(avatarLoader);
            root.Add(avatarManager);
            root.Add(motionSource);

            var avatarCollectionIdField1 = new LongField
            {
                label = "Avatar Collection ID"
            };
            
            var metadataAssetUriField = new TextField
            {
                label = "Metadata Asset Uri"
            };
            
            var tokenIdField1 = new TextField
            {
                label = "Token ID"
            };
            
            var tokenAssetUriField = new TextField
            {
                label = "Token Asset Uri"
            };
            
            var loadAvatarButton = new Button
            {
                text = "Load Avatar"
            };
            
            loadAvatarButton.clicked += (() =>
            {
                var obj = new LoadAvatarMessage { 
                    avatarCollectionId = avatarCollectionIdField1.value,
                    metadataAssetUri = metadataAssetUriField.value,
                    tokenId = tokenIdField1.value,
                    tokenAssetUri = tokenAssetUriField.value
                };
                
                var message = JsonConvert.SerializeObject(obj);
                (target as MessageHandler.MessageHandler)!.LoadAvatar(message);
            });

            var avatarCollectionIdField2 = new LongField
            {
                label = "Avatar Collection ID"
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
                var obj = new SelectAvatarMessage
                    {
                        avatarCollectionId = avatarCollectionIdField2.value,
                        tokenId = tokenIdField.value
                    };
                var message = JsonConvert.SerializeObject(obj);
                (target as MessageHandler.MessageHandler)!.SelectAvatar(message);
            };

            var switchModeButton = new Button
            {
                text = "Switch Mode"
            };
            
            switchModeButton.clicked += () =>
            {
                (target as MessageHandler.MessageHandler)!.SwitchMode("");
            };

            var motionCapturedInput = new TextField
            {
                label = "MotionCaptured"
            };
            
            var captureMotion = new Button
            {
                text = "Capture Motion"
            };
            
            captureMotion.clicked += () =>
            {
                (target as MessageHandler.MessageHandler)!.ProcessCapturedResult(motionCapturedInput.value);
            };

            var syncedBlinkScale = new Slider("SyncedBlink", 0f, 1.0f);
            syncedBlinkScale.value = 0.5f;
            syncedBlinkScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdateSyncedBlinkScale(evt.newValue);
            });

            var blinkScale = new Slider("Blink", 0f, 2.0f);
            blinkScale.value = 1.0f;
            blinkScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdateBlinkScale(evt.newValue);
            });

            var pupilScale = new Slider("Pupil", 0f, 2.0f);
            pupilScale.value = 1.0f;
            pupilScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdatePupilScale(evt.newValue);
            });

            var eyebrowScale = new Slider("Eyebrow", 0f, 2.0f);
            eyebrowScale.value = 1.0f;
            eyebrowScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdateEyebrowScale(evt.newValue);
            });

            var mouthXScale = new Slider("MouthX", 0f, 2.0f);
            mouthXScale.value = 1.0f;
            mouthXScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdateMouthXScale(evt.newValue);
            });

            var mouthYScale = new Slider("MouthY", 0f, 2.0f);
            mouthYScale.value = 1.0f;
            mouthYScale.RegisterValueChangedCallback((evt) =>
            {
                (target as MessageHandler.MessageHandler)!.UpdateMouthYScale(evt.newValue);
            });

            root.Add(avatarCollectionIdField1);
            root.Add(tokenIdField1);
            root.Add(metadataAssetUriField);
            root.Add(tokenAssetUriField);
            root.Add(loadAvatarButton);

            root.Add(avatarCollectionIdField2);
            root.Add(tokenIdField);
            root.Add(selectAvatar);

            root.Add(switchModeButton);

            root.Add(motionCapturedInput);
            root.Add(captureMotion);
            
            root.Add(syncedBlinkScale);
            root.Add(blinkScale);
            root.Add(pupilScale);
            root.Add(eyebrowScale);
            root.Add(mouthXScale);
            root.Add(mouthYScale);
            
            return root;
        }
    }
}