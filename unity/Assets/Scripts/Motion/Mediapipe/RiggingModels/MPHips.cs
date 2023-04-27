using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels
{
    public class MPHips : MPJointModel
    {
        protected override void Process()
        {
            if (rawPoints == null) return;
            var hipLr = rawPoints[23] - rawPoints[24];
            up = Vector3.up;
            hipLr.Normalize();
            lookAt = Vector3.Cross(hipLr, up);
            up = Vector3.Cross(lookAt, hipLr);
            lookAt.z = -lookAt.z;
            up.z = -up.z;
        }
    }
}
