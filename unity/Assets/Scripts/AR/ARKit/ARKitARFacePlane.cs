using MYTYKit.MotionTemplates;
using UnityEngine;

namespace AR.ARKit
{
    public class ARKitARFacePlane : ARFacePlane
    {
        [SerializeField]
        Camera m_mainCamera;
        [SerializeField]
        GameObject m_renderingObjects;
        
        protected override void UpdateFromMotionTemplate()
        {
            var anchorTemplate = motionTemplateMapper.GetTemplate("Head") as AnchorTemplate;
            
            var toWorldPoint = m_mainCamera.ViewportToWorldPoint(new Vector3(anchorTemplate!.position.x, 1 - anchorTemplate.position.y, m_mainCamera.nearClipPlane));
            var renderingObjectsPosition = m_renderingObjects.transform.position;
            
            facePlane.localPosition = new Vector3(toWorldPoint.x - renderingObjectsPosition.x, toWorldPoint.y- renderingObjectsPosition.y, -1f);

            facePlane.localScale = anchorTemplate.scale;
        }
    }
}