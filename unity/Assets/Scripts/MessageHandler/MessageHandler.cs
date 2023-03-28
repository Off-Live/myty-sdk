using System;
using Avatar;
using UnityEngine;

namespace MessageHandler
{
    public class MessageHandler : MonoBehaviour
    {
        public AvatarLoader avatarLoader;
        public Avatar3DLoader avatar3DLoader;
        public AvatarManager avatarManager;
        private void Start()
        {
            avatarLoader = FindObjectOfType<AvatarLoader>();
            avatar3DLoader = FindObjectOfType<Avatar3DLoader>();
            avatarManager = FindObjectOfType<AvatarManager>();
        }

        public void LoadAvatar(
            long assetVersionId,
            string templateAssetUri,
            string tokenId,
            string tokenAssetUri
        )
        {
            avatarLoader.LoadAvatar(assetVersionId, templateAssetUri, tokenId, tokenAssetUri);
        }

        public void SelectAvatar(long assetVersionId, string tokenID)
        {
            avatarManager.SelectAvatar(assetVersionId, tokenID);
        }

        public void SetARMode(bool flag)
        {
            avatarManager.SetARMode(flag);
        }

        public void Load3DAvatar()
        {
            avatar3DLoader.LoadAvatar();
        }
    }
}