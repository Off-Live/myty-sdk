using MYTYKit.MotionTemplates;
using UnityEngine;

namespace AR.Mocap4Face
{
    public class M4FARFacePlane : ARFacePlane
    {
        protected override void UpdateFromMotionTemplate()
        {
            var template = motionTemplateMapper.GetTemplate("Head") as AnchorTemplate;
            var position = template!.position;
            
            var bounds = arBounds.bounds;
            var calculatedPosition = new Vector3(-(position.x - 0.5f) * bounds.size.x,
                (position.y - 0.5f) * bounds.size.y,
                0.5f);
            
            facePlane.localPosition = new Vector3(calculatedPosition.x, calculatedPosition.y, -5);
            facePlane.localScale = template!.scale * 15;
        }
    }
}