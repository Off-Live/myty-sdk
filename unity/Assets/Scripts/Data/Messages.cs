namespace Data
{
    public class LoadAvatarMessage
    {
        public long assetVersionId;
        public string templateAssetUri;
        public string tokenId;
        public string tokenAssetUri;
    }

    public class SelectAvatarMessage
    {
        public long assetVersionId;
        public string tokenId;
    }
}