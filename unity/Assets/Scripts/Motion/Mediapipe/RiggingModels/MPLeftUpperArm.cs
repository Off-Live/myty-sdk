using UnityEngine;

namespace Motion.Mediapipe.RiggingModels
{
    public class MPLeftUpperArm : MPJointModel
    {
        Vector3 m_lastLookAt;

        void Start()
        {
            m_lastLookAt = Vector3.up;
        }

        protected override void Process()
        {
            if (rawPoints == null) return;

            var upperArm = rawPoints[13] - rawPoints[11];
            var lowerArm = rawPoints[15] - rawPoints[13];

            upperArm.Normalize();
            lowerArm.Normalize();

            lookAt = -Vector3.Cross(upperArm, lowerArm);
            if (lookAt.sqrMagnitude < 1.0e-6)
            {
                lookAt = m_lastLookAt;
            }
            
            lookAt.Normalize();
            up = upperArm.normalized;
            m_lastLookAt = lookAt;
            lookAt.z = -lookAt.z;
            up.z = -up.z;
        }
    }
}