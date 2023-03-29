using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels
{
    public abstract class MPJointModel: MPBaseModel
    {
        protected Vector3 up, lookAt;

        protected override void UpdateTemplate()
        {
            if (templateList.Count == 0) return;
            foreach (var motionTemplate in templateList)
            {
                var anchorTemplate = (AnchorTemplate)motionTemplate;
                anchorTemplate.up = up;
                anchorTemplate.lookAt = lookAt;
                anchorTemplate.NotifyUpdate();
            }
        }
    }
}