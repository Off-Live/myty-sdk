using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.ARKit.RiggingModels
{
    public class ARHeadRotation : MotionTemplateBridge
    {
        public Vector3 up, lookAt, position, scale;
        Vector3 m_up, m_lookAt, m_position, m_scale;

        protected override void Process()
        {
            m_up = up;
            m_lookAt = lookAt;
            m_position = position;
            m_scale = scale;
        }

        protected override void UpdateTemplate()
        {
            foreach (var template in templateList)
            {
                var anchor = template as AnchorTemplate;
                anchor.up = m_up;
                anchor.lookAt = m_lookAt;
                anchor.position = m_position;
                anchor.scale = m_scale;
                anchor.NotifyUpdate();
            }
        }
    }
}