using UnityEngine;

namespace Avatar.Interface
{
    public abstract class AvatarDownloader : MonoBehaviour 
    {
        public abstract void DownloadAvatar(
            long avatarCollectionId,
            string metadataAssetUri,
            string tokenId,
            string tokenAssetUri);
    }
}