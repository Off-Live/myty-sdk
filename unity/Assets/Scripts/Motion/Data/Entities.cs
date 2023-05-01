using System.Collections.Generic;
using UnityEngine;

namespace Motion.Data
{
    public class BridgeItemWithName
    {
        public string name;
        public BridgeItem result;
    }

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

    public class CapturedResultWithName
    {
        public string name;
        public string result;
    }
}