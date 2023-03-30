using System.Linq;
using Data;
using MotionSource.Mocap4Face.RiggingModels;
using Newtonsoft.Json;
using UnityEngine;

namespace MotionSource.Mocap4Face
{
    public class M4FMotionSource : MotionSource
    {
        public override void ProcessCapturedResult(string result)
        {
            var obj = JsonConvert.DeserializeObject<M4FData>(result);

            var faceBridges = GetBridgesInCategory("FaceLandmark");
            foreach (var headPose in faceBridges.Select(bridge => bridge as M4FHead).Where(_ => _ != null))
            {
                headPose.eularAngleInRadian = obj!.headPose;
                headPose.normalizedPosition = obj!.normalizedPosition;
                headPose.normalizedScale = obj!.normalizedScale;
                headPose.Flush();
            }

            var solverBridges = GetBridgesInCategory("FaceSolver");
            foreach (var face in solverBridges.Select(bridge => bridge as M4FSimpleFace).Where(_ => _ != null))
            {
                foreach (var item in obj!.blendshapes)
                {
                    face.SetBlendshape(item.name, item.value);
                }

                face.Flush();
            }

            var poseBridges = GetBridgesInCategory("PoseLandmark");
            foreach (var chest in poseBridges.Select(bridge => bridge as M4FChest).Where(_ => _ != null))
            {
                chest.lookAtVector = 3 * (obj!.normalizedPosition - new Vector2(0.5f, 0.5f));
                chest.Flush();
            }
        }
    }
}