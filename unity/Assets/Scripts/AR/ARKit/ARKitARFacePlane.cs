using MYTYKit.MotionTemplates;
using UnityEngine;

namespace AR.ARKit
{
    public class ARKitARFacePlane : ARFacePlane
    {
        protected override void UpdateFromMotionTemplate()
        {
            var anchorTemplate = motionTemplateMapper.GetTemplate("Head") as AnchorTemplate;
            var bounds = arBounds.bounds;
            
            facePlane.localPosition = new Vector3(
                -(anchorTemplate.position.x - 0.5f) * bounds.size.x,
                (anchorTemplate.position.y - 0.5f) * bounds.size.y, 
                -1);
            
            facePlane.localScale = anchorTemplate.scale;
        }
    }
}