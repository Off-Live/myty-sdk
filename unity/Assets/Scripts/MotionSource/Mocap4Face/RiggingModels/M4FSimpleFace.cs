using System.Collections.Generic;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.Mocap4Face.RiggingModels
{
    public class M4FSimpleFace : MotionTemplateBridge
    {
        Dictionary<string, float> m_blendshapes = new();

        public float leftEye;
        public float rightEye;

        public float leftEyeBrow;
        public float rightEyeBrow;

        public Vector2 leftPupil;
        public Vector2 rightPupil;

        public float mouthX;
        public float mouthY;

        public void SetBlendshape(string name, float value)
        {
            m_blendshapes[name] = value;
        }

        protected override void UpdateTemplate()
        {
            if (templateList.Count == 0) return;

            foreach (var motionTemplate in templateList)
            {
                var template = (ParametricTemplate)motionTemplate;

                template.SetValue("leftEye", leftEye);
                template.SetValue("rightEye", rightEye);
                template.SetValue("leftEyeBrow", leftEyeBrow);
                template.SetValue("rightEyeBrow", rightEyeBrow);
                template.SetValue("leftPupilX", leftPupil.x);
                template.SetValue("leftPupilY", leftPupil.y);
                template.SetValue("rightPupilX", rightPupil.x);
                template.SetValue("rightPupilY", rightPupil.y);
                template.SetValue("mouthX", mouthX);
                template.SetValue("mouthY", mouthY);
                template.NotifyUpdate();
            }
        }

        protected override void Process()
        {
            leftEye = 1.0f - m_blendshapes["eyeBlink_L"];
            rightEye = 1.0f - m_blendshapes["eyeBlink_R"];

            var mouthPucker = m_blendshapes["mouthPucker"];
            var mouthLeftHalf = Mathf.Max(m_blendshapes["mouthUpperUp_L"], m_blendshapes["mouthSmile_L"]);

            var mouthRightHalf = Mathf.Max(m_blendshapes["mouthUpperUp_R"], m_blendshapes["mouthSmile_R"]);

            var mouthNeutralX = 0.4f;

            if (mouthPucker > 0.15f)
            {
                mouthX = (1 - mouthPucker) * mouthNeutralX;
            }
            else
            {
                mouthNeutralX = (1 - mouthPucker) * mouthNeutralX;
                mouthX = mouthNeutralX + (mouthLeftHalf + mouthRightHalf) * (1 - mouthNeutralX) * 0.5f;
            }

            mouthY = m_blendshapes["jawOpen"];

            var eyebrowNeutral = 0.5f;

            leftEyeBrow = (1.0f - m_blendshapes["browDown_L"]) * eyebrowNeutral +
                          m_blendshapes["browOuterUp_L"] * (1.0f - eyebrowNeutral);
            rightEyeBrow = (1.0f - m_blendshapes["browDown_R"]) * eyebrowNeutral +
                           m_blendshapes["browOuterUp_R"] * (1.0f - eyebrowNeutral);
        }
    }
}