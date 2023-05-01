using Data;
using Motion.MotionTemplateBridge;
using UnityEngine;

namespace Motion.ARKit.RiggingModels
{
    public class ARKitSimpleFace : ParametricBridge
    {
        public float leftEye;
        public float rightEye;

        public float leftEyeBrow;
        public float rightEyeBrow;

        public Vector2 leftPupil = Vector2.zero;
        public Vector2 rightPupil = Vector2.zero;

        public float mouthX;
        public float mouthY;

        ARKitBlendshape m_blendshapes;
        
        public void SetBlendshapes(ARKitBlendshape blendshape)
        {
            m_blendshapes = blendshape;
        }
        protected override void Process()
        {
            leftEye = 1.0f - m_blendshapes.eyeblinkLeft;
            rightEye = 1.0f - m_blendshapes.eyeblinkRight;

            var mouthPouker = m_blendshapes.mouthPucker;
            var mouthLeftHalf = Mathf.Max(m_blendshapes.mouthStretchLeft, m_blendshapes.mouthSmileLeft);
                            
            var mouthRightHalf = Mathf.Max(m_blendshapes.mouthStretchRight, m_blendshapes.mouthSmileRight);
                             
            var mouthNeutralX = 0.4f;

            if (mouthPouker > 0.15f)
            {
                mouthX = (1 - mouthPouker) * mouthNeutralX;
            }
            else
            {
                mouthNeutralX = (1 - mouthPouker) * mouthNeutralX;
                mouthX = mouthNeutralX + (mouthLeftHalf + mouthRightHalf) * (1 - mouthNeutralX) * 0.5f;
            }

            mouthY = m_blendshapes.jawOpen - m_blendshapes.mouthClose;

            var eyebrowNeutral = 0.5f;

            leftEyeBrow = (1.0f - m_blendshapes.browDownLeft) * eyebrowNeutral +
                          m_blendshapes.browOuterUpLeft * (1.0f - eyebrowNeutral);
            rightEyeBrow = (1.0f - m_blendshapes.browDownRight) * eyebrowNeutral +
                           m_blendshapes.browOuterUpRight * (1.0f - eyebrowNeutral);
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