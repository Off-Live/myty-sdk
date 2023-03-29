using Avatar;
using Data;
using MotionSource;
using Newtonsoft.Json;
using UnityEngine;

namespace MessageHandler
{
    public class MessageHandler : MonoBehaviour
    {
        public AvatarLoader avatarLoader;
        public Avatar3DLoader avatar3DLoader;
        public AvatarManager avatarManager;
        public MotionProcessor motionProcessor;
        private void Start()
        {
            avatarLoader = FindObjectOfType<AvatarLoader>();
            avatar3DLoader = FindObjectOfType<Avatar3DLoader>();
            avatarManager = FindObjectOfType<AvatarManager>();
            motionProcessor = FindObjectOfType<MotionProcessor>();
        }

        public void LoadAvatar(string message)
        {
            var obj = JsonConvert.DeserializeObject<LoadAvatarMessage>(message);
            avatarLoader.LoadAvatar(
                obj!.assetVersionId,
                obj!.templateAssetUri,
                obj!.tokenId,
                obj!.tokenAssetUri
            );
        }

        public void SelectAvatar(string message)
        {
            var obj = JsonConvert.DeserializeObject<SelectAvatarMessage>(message);
            avatarManager.SelectAvatar(
                obj!.assetVersionId,
                obj!.tokenId
            );
        }

        public void SetARMode(string flag)
        {
            avatarManager.SetARMode(flag == "true");
        }

        public void Load3DAvatar()
        {
            avatar3DLoader.LoadAvatar();
        }

        public void Load3DTraits()
        {
            avatar3DLoader.LoadTraits();
        }

        public void ProcessMediapipe(string message)
        {
            var obj = JsonConvert.DeserializeObject<MediapipeData>(message);
            motionProcessor.ProcessFaceMediapipe(obj!.face, obj!.width, obj!.height);
            motionProcessor.ProcessPoseMediapipe(obj!.pose);
        }

        public void ProcessMocap4Face(string json)
        {
            Debug.Log(json);
        }
    }
}