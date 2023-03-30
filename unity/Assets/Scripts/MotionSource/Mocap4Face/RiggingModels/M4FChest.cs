using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.Mocap4Face.RiggingModels
{
    public class M4FChest : MotionTemplateBridge
    {
        public Vector3 lookAtVector;
        
        Vector3 m_up, m_lookAt;
        protected override void UpdateTemplate()
        {
            if (templateList.Count == 0) return;
            foreach (var motionTemplate in templateList)
            {
                var anchorTemplate = (AnchorTemplate)motionTemplate;
                anchorTemplate.up = m_up;
                anchorTemplate.lookAt = m_lookAt;
                anchorTemplate.NotifyUpdate();
            }
        }

        protected override void Process()
        {
            m_lookAt = lookAtVector;
            m_up = Vector3.zero;
        }
    }
}