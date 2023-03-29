using System.Collections.Generic;
using UnityEngine;

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

    public class MediapipeData
    {
        public List<Vector3> face;
        public List<Vector3> pose;
        public int width;
        public int height;
    }
}