using Motion.Data;
using Motion.MotionTemplateBridge;
using UnityEngine;

namespace Motion.Mediapipe.RiggingModels.ARFace
{
    public class MPARFace : PointsBridge
    {
        public Vector3 normalizedPosition;
        public Vector3 normalizedScale;
        protected override void UpdateValue()
        {
        }

        protected override void Process()
        {
            if(rawPoints.Length < 468) return;
            var sumPosition = Vector3.zero;

            for (var i = 0; i < 468; i++)
            {
                sumPosition += rawPoints[i];
            }
            
            sumPosition /= 468;

            normalizedPosition = new Vector3(sumPosition.x, sumPosition.y, -1);
            
            var height = (rawPoints[10] - rawPoints[152]).magnitude;
            var width = (rawPoints[454] - rawPoints[234]).magnitude;
            var size = (width + height) / 2 * 0.3f;
            
            normalizedScale = new Vector3(size, size, size);
        }

        public override BridgeItem CreateItem()
        {
            return new AnchorBridgeItem
            {
                up = Vector3.zero,
                lookAt = Vector3.zero,
                position = normalizedPosition,
                scale = normalizedScale
            };
        }
    }
}