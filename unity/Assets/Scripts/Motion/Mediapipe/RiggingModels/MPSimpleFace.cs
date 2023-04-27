using MotionSource._3rdParty.MeFaMo;
using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels
{
    public class MPSimpleFace : MPSolverModel
    {
        public float leftEye;
        public float rightEye;

        public float leftEyeBrow;
        public float rightEyeBrow;

        public Vector2 leftPupil;
        public Vector2 rightPupil;

        public float mouthX;
        public float mouthY;

        protected override void Process()
        {
            if (m_solver == null) return;
            leftPupil = m_solver.leftPupil;
            rightPupil = m_solver.rightPupil;

            leftEye = 1.0f - m_solver.blendShape[MeFaMoConfig.FaceBlendShape.EyeBlinkLeft];
            rightEye = 1.0f - m_solver.blendShape[MeFaMoConfig.FaceBlendShape.EyeBlinkRight];

            var mouthPouker = m_solver.blendShape[MeFaMoConfig.FaceBlendShape.MouthPucker];
            var mouthLeftHalf = m_solver.blendShape[MeFaMoConfig.FaceBlendShape.MouthSmileLeft] +
                                m_solver.blendShape[MeFaMoConfig.FaceBlendShape.MouthStretchLeft];
            var mouthRightHalf = m_solver.blendShape[MeFaMoConfig.FaceBlendShape.MouthSmileRight] +
                                m_solver.blendShape[MeFaMoConfig.FaceBlendShape.MouthStretchRight];
            var mouthNeutralX = 0.4f;

            if (mouthPouker > 0)
            {
                mouthX = (1 - mouthPouker) * mouthNeutralX;
            }
            else
            {
                mouthX = mouthNeutralX + (mouthLeftHalf + mouthRightHalf) * (1 - mouthNeutralX) * 0.5f;
            }
            mouthY = m_solver.blendShape[MeFaMoConfig.FaceBlendShape.JawOpen];

            var eyebrowNeutral = 0.5f;
            leftEyeBrow = (1.0f- m_solver.blendShape[MeFaMoConfig.FaceBlendShape.BrowDownLeft]/0.4f) * eyebrowNeutral +
                          m_solver.blendShape[MeFaMoConfig.FaceBlendShape.BrowOuterUpLeft]*(1.0f-eyebrowNeutral);
            rightEyeBrow = (1.0f- m_solver.blendShape[MeFaMoConfig.FaceBlendShape.BrowDownRight]/0.4f) * eyebrowNeutral +
                             m_solver.blendShape[MeFaMoConfig.FaceBlendShape.BrowOuterUpRight]*(1.0f-eyebrowNeutral);
            m_solver = null;
        }

        protected override void UpdateValue()
        {
            SetValue("leftEye",leftEye);
            SetValue("rightEye",rightEye);
            SetValue("leftEyeBrow", leftEyeBrow);
            SetValue("rightEyeBrow", rightEyeBrow);
            SetValue("leftPupilX", leftPupil.x);
            SetValue("leftPupilY", leftPupil.y);
            SetValue("rightPupilX", rightPupil.x);
            SetValue("rightPupilY", rightPupil.y);
            SetValue("mouthX", mouthX);
            SetValue("mouthY", mouthY);
        }
    }
}