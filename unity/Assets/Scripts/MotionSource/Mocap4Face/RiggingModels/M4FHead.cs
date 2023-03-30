using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.Mocap4Face.RiggingModels
{
    public class M4FHead : MotionTemplateBridge
    {
        public Vector3 eularAngleInRadian;
        public Vector2 normalizedPosition;
        public float normalizedScale;

        Vector3 m_up;
        Vector3 m_lookAt;
        Vector3 m_normalizedPosition;
        Vector3 m_normalizedScale;
        protected override void UpdateTemplate()
        {
            if (templateList.Count == 0) return;
            foreach (var motionTemplate in templateList)
            {
                var anchorTemplate = (AnchorTemplate)motionTemplate;
                anchorTemplate.up = m_up;
                anchorTemplate.lookAt = m_lookAt;
                anchorTemplate.position = m_normalizedPosition;
                anchorTemplate.scale = m_normalizedScale;
                anchorTemplate.NotifyUpdate();
            }
        }
        protected override void Process()
        {
            var rotation = Quaternion.Euler(eularAngleInRadian * 180 / Mathf.PI);
            m_up = rotation * Vector3.up;
            m_lookAt = rotation * Vector3.forward;
            m_normalizedPosition = normalizedPosition;
            m_normalizedScale =  new Vector3(normalizedScale, normalizedScale, normalizedScale);    
        }
    }
}