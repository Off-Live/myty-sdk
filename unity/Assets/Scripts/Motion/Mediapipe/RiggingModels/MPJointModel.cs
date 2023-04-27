using Motion.Data;
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

        public override BridgeItem CreateItem()
        {
            return new AnchorBridgeItem
            {
                up = up,
                lookAt = lookAt,
                position = null,
                scale = null
            };
        }
    }
}