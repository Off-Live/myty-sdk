using System;
using UnityEngine;

namespace Motion.MotionSource
{
    [Serializable]
    public class MotionSourceItem
    {
        public string name;
        public MotionSource motionSource;
    }
    public class MotionSourceContainer : MonoBehaviour
    {
        public MotionProcessor.MotionProcessor motionProcessor;
        
        
    }
}