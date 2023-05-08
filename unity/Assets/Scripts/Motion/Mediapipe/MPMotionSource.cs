using System.Collections.Generic;
using System.Linq;
using Data;
using Motion._3rdParty.MeFaMo;
using Motion.Mediapipe.RiggingModels;
using Motion.MotionTemplateBridge;
using Newtonsoft.Json;
using UnityEngine;

namespace Motion.Mediapipe
{
    public class MPMotionSource : Motion.MotionSource.MotionSource
    {
        MeFaMoSolver m_solver = new();
        Vector3[] m_solverBuffer;
        public MeshRenderer arBounds;
        protected override void ConvertCapturedResult(string result)
        {
            var obj = JsonConvert.DeserializeObject<MediapipeData>(result);
            
            var faceData = obj!.face;
            
            var faceBridges = GetBridgesInCategory("FaceLandmark");

            foreach (var baseModel in faceBridges.Select(model => model as PointsBridge))
            {
                ProcessNormalizedHolistic(baseModel, faceData);
            }

            if (m_solverBuffer == null || m_solverBuffer.Length != faceData.Count)
            {
                m_solverBuffer = new Vector3[faceData.Count];
            }

            foreach (var (elem, index) in faceData.Select((item, idx) => (item, idx)))
            {
                m_solverBuffer[index] = elem;
            }
            
            var (width, height) = (obj!.width, obj!.height);
            
            m_solver.Solve(m_solverBuffer, width, height);
            
            var solverBridges = GetBridgesInCategory("FaceSolver");

            foreach (var model in solverBridges)
            {
                var solverModel = model as MPSolverModel;
                if (solverModel == null) continue;
                solverModel.SetSolver(m_solver);
                solverModel.Flush();
            }

            var arFaceBridges = GetBridgesInCategory("ARFace");
            foreach (var model in arFaceBridges.Select(model => model as PointsBridge))
            {
                ProcessNormalizedRelativeToBounds(model, faceData);
            }
            
            var pose = obj!.pose;
            
            var poseBridges = GetBridgesInCategory("PoseLandmark");
            foreach (var model in poseBridges.Select(model => model as PointsBridge))
            {
                ProcessNormalizedHolistic(model, pose);
            }
        }

        public override float GetWorldPointHeight()
        {
            return arBounds.bounds.size.x;
        }

        public override float GetWorldPointWidth()
        {
            return arBounds.bounds.size.y;
        }

        private void ProcessNormalizedHolistic(PointsBridge model, List<Vector3> landmarkList)
        {
            if (model == null || landmarkList == null) return;

            if (model.GetNumPoints() != landmarkList.Count)
            {
                model.Alloc(landmarkList.Count);
            }

            foreach (var (elem, index) in landmarkList.Select((item, idx) => (item, idx)))
            {
                model.SetPoint(index, elem, 1.0f);
            }
        
            model.Flush();
        }

        private void ProcessNormalizedRelativeToBounds(PointsBridge model, List<Vector3> landmarkList)
        {
            if (model == null || landmarkList == null) return;

            if (model.GetNumPoints() != landmarkList.Count)
            {
                model.Alloc(landmarkList.Count);
            }

            var bounds = arBounds.bounds;
            foreach (var (elem, index) in landmarkList.Select((item, idx) => (item, idx)))
            {
                model.SetPoint(index, new Vector3((-elem.x + 0.5f) * bounds.size.x,
                    -(-elem.y + 0.5f) * bounds.size.y,
                    elem.z + 0.5f
                    ), 1.0f);
            }
            
            model.Flush();
        }
    }
}