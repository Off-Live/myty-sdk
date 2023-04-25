using UnityEngine;

namespace Avatar.Interface
{
    public abstract class AvatarManager : MonoBehaviour
    {
        public MotionSource.MotionSource motionSource;
        public abstract void LoadAvatar(long avatarCollectionId, string tokenId, byte[] metadata, byte[] tokenAsset);
        public abstract void SelectAvatar(long avatarCollectionId, string tokenId);
        public abstract void SwitchMode();
        public abstract bool IsMetadataLoaded(long avatarCollectionId);
        public abstract bool IsAvatarLoaded(long avatarCollectionId, string tokenId);
    }
}