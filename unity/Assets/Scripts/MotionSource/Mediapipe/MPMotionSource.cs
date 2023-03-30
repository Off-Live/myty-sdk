using System.Collections.Generic;
using System.Linq;
using Data;
using MotionSource._3rdParty.MeFaMo;
using MotionSource.Mediapipe.RiggingModels;
using Newtonsoft.Json;
using UnityEngine;

namespace MotionSource.Mediapipe
{
    public class MPMotionSource : MotionSource
    {
        MeFaMoSolver m_solver = new();
        Vector3[] m_solverBuffer;
        public override void ProcessCapturedResult(string result)
        {
            var obj = JsonConvert.DeserializeObject<MediapipeData>(result);
            
            var faceData = obj!.face;
            
            var faceBridges = GetBridgesInCategory("FaceLandmark");

            foreach (var baseModel in faceBridges.Select(model => model as MPBaseModel))
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
            
            var pose = obj!.pose;
            
            var poseBridges = GetBridgesInCategory("PoseLandmark");
            foreach (var model in poseBridges.Select(model => model as MPBaseModel))
            {
                ProcessNormalizedHolistic(model, pose);
            }
        }

        private void ProcessNormalizedHolistic(MPBaseModel model, List<Vector3> landmarkList)
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
    }
}