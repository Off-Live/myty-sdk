using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels.RightHand
{
    public class MPRightThumbProximal : MPJointModel
    {
        protected override void Process()
        {
            if (rawPoints == null) return;

            var proximal = rawPoints[2] - rawPoints[1];
            var indexprox = rawPoints[5] - rawPoints[1];
            var thumbPlane = Vector3.Cross(proximal, indexprox);

            thumbPlane.Normalize();
            indexprox.Normalize();
            proximal.Normalize();

            up = proximal;
            lookAt = Vector3.Cross(thumbPlane, proximal);
        }
    }
}