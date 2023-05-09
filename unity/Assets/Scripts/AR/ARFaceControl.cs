using Motion.MotionSource;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace AR
{
    public class ARFaceControl : MonoBehaviour
    {
        public MotionTemplateMapper motionTemplateMapper;
        
        public MotionSource motionSource;

        public Transform facePlane;
        
        private float m_scale = 1.0f;
        private float m_xOffset = 0;
        private float m_yOffset = 0;

        void Update()
        {
            UpdateFromMotionTemplate();
        }

        private void UpdateFromMotionTemplate()
        {
            var motionTemplate = motionTemplateMapper.GetTemplate("NormalizedARFace") as AnchorTemplate;
            
            var width = motionSource.GetWorldPointWidth();
            var height = motionSource.GetWorldPointHeight();
            
            facePlane.localPosition = new Vector3(
                motionTemplate!.position.x + m_xOffset * width / 2,
                motionTemplate!.position.y + m_yOffset * height / 2,
                motionTemplate!.position.z);
            facePlane.localScale = motionTemplate!.scale * m_scale;
        }

        public void SetXOffset(float value)
        {
            m_xOffset = value;
        }

        public void SetYOffset(float value)
        {
            m_yOffset = value;
        }

        public void SetScale(float value)
        {
            m_scale = value;
        }

        public void SetControlAsDefault()
        {
            m_xOffset = 0;
            m_yOffset = 0;
            m_scale = 1.0f;
        }
    }
}