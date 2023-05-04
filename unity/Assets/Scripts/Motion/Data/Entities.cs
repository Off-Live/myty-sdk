using System.Collections.Generic;
using UnityEngine;

namespace Motion.Data
{
    public abstract class BridgeItem
    {
    }

    public class PointsBridgeItem : BridgeItem
    {
        public Vector3[] rawPoints;
        public float[] visibilities;
    }
    
    public class ParametricBridgeItem : BridgeItem
    {
        public Dictionary<string, float> parametricItems;
    }
    
    public class AnchorBridgeItem : BridgeItem
    {
        public Vector3 up;
        public Vector3 lookAt;
        public Vector3? position;
        public Vector3? scale;
    }

    public class CalibrationItem
    {
        public float blinkScale = 1.0f;
        public float syncedBlinkScale = 0.5f;
        public float pupilScale = 1.0f;
        public float eyebrowScale = 1.0f;
        public float mouthXScale = 1.0f;
        public float mouthYScale = 1.0f;
    }
}