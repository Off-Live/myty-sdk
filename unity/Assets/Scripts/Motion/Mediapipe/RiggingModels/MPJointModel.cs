using Motion.MotionTemplateBridge;
using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels
{
    public abstract class MPJointModel: PointsBridge
    {
        protected Vector3 up, lookAt;

        protected override void UpdateValue()
        {
            
        }
    }
}