using UnityEngine;

namespace Motion.Mediapipe.RiggingModels
{
    public class MPLeftUpperLeg : MPJointModel
    {
        Vector3 m_lastLA;

        void Start()
        {
            m_lastLA = Vector3.forward;
        }

        protected override void Process()
        {
            if (rawPoints == null) return;
            var upperLeg = rawPoints[23] - rawPoints[25];
            var lowerLeg = rawPoints[25] - rawPoints[27];
            upperLeg.Normalize();
            lowerLeg.Normalize();

            up = upperLeg;
            var axis = Vector3.Cross(lowerLeg, upperLeg);
            if (axis.magnitude < 1.0e-6)
            {
                lookAt = m_lastLA;
            }
            else
            {
                lookAt = Vector3.Cross(axis, up);
                lookAt.Normalize();
                m_lastLA = lookAt;
            }
        }
    }
}
