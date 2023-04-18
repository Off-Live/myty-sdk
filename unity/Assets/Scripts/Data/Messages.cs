using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

    public class ARKitData
    {
        public Vector3 facePosition;
        public Vector3 faceScale;
        public Vector3 up;
        public Vector3 forward;
        public ARKitBlendshape blendshapes;
    }

    public class ARKitBlendshape
    {
        public float eyeblinkLeft;
        public float eyeblinkRight;
        public float mouthPucker;
        public float mouthStretchLeft;
        public float mouthSmileLeft;
        public float mouthStretchRight;
        public float mouthSmileRight;
        public float jawOpen;
        public float mouthClose;
        public float browDownLeft;
        public float browOuterUpLeft;
        public float browDownRight;
        public float browOuterUpRight;
    }
}