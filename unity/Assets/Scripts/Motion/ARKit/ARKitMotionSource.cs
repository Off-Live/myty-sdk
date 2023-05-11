using System.Linq;
using Data;
using Motion.ARKit.RiggingModels;
using Newtonsoft.Json;
using UnityEngine;

namespace Motion.ARKit
{
    public class ARKitMotionSource : Motion.MotionSource.MotionSource
    {
        public Camera mainCamera;
        public GameObject renderingObjects;
        protected override void ConvertCapturedResult(string result)
        {
            var obj = JsonConvert.DeserializeObject<ARKitData>(result);
            
            var headBridge = GetBridgesInCategory("Head");
            foreach (var arHeadRotation in headBridge.Select(bridge => bridge as ARKitHeadRotation))
            {
                arHeadRotation.up = obj!.up;
                arHeadRotation.lookAt = obj!.forward;
                arHeadRotation.Flush();
            }
            
            var parametricBridge = GetBridgesInCategory("Face");
            foreach (var simpleFace in parametricBridge.Select(bridge => bridge as ARKitSimpleFace))
            {
                simpleFace.SetBlendshapes(obj!.blendshapes);
            }

            foreach (var bridge in parametricBridge)
            {
                bridge.Flush();
            }
            
            var arFaceBridge = GetBridgesInCategory("ARFace");
            foreach (var arFace in arFaceBridge.Select(bridge => bridge as ARKitARFace))
            {
                var facePos = obj!.facePosition;
                var toWorldPoint = mainCamera.ViewportToWorldPoint(
                    new Vector3(facePos.x, 1 - facePos.y, mainCamera.nearClipPlane)
                    );
                var renderingObjectsPos = renderingObjects.transform.position;
                arFace.position = new Vector3(toWorldPoint.x - renderingObjectsPos.x, toWorldPoint.y - renderingObjectsPos.y, -1f);
                arFace.scale = obj!.faceScale;
                arFace.Flush();
            }
        }

        public override float GetWorldPointWidth()
        {
            return GetWorldPointHeight() * mainCamera.aspect;
        }

        public override float GetWorldPointHeight()
        {
            return mainCamera.orthographicSize * 2;
        }
    }
}