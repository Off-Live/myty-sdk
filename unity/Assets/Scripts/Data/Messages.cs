using System;
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
        public bool m4fEnabled;
        public List<Vector3> face;
        public List<Vector3> pose;
        public int width;
        public int height;
    }
    
    public class M4FData
    {
        [Serializable]
        public class M4FEntry
        {
            public string name;
            public float value;
        }

        public List<M4FEntry> blendshapes;
        public Vector3 headPose;
        public Vector2 normalizedPosition;
        public float normalizedScale;
    }
}