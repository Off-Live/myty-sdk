using System.Collections.Generic;
using System.Linq;
using MotionSource._3rdParty.MeFaMo;
using MotionSource.Mediapipe.RiggingModels;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.MotionProcessor
{
    public class MotionProcessor : MonoBehaviour
    {
        [SerializeField]
        Motion.MotionSource.MotionSource m_motionSource;
        
        MeFaMoSolver m_solver = new();
        Vector3[] m_solverBuffer;
        
        List<MotionTemplateMapper> m_motionTemplateMapperList;

        public void UpdateTemplateByName(string templateName, string result)
        {
            foreach (var motionTemplateMapper in m_motionTemplateMapperList)
            {
                var template = motionTemplateMapper.GetTemplate(templateName);
                if(template == null) continue;

                switch (template)
                {
                    case PointsTemplate pointsTemplate:
                        pointsTemplate.points = new Vector3[1];
                        pointsTemplate.visibilities = new float[1];
                        break;
                    case AnchorTemplate anchorTemplate:
                        anchorTemplate.up = new Vector3();
                        anchorTemplate.scale = Vector3.one;
                        anchorTemplate.position = Vector3.one;
                        anchorTemplate.lookAt = Vector3.one;
                        break;
                    case ParametricTemplate parametricTemplate:
                        parametricTemplate.SetValue("key", 0.0f);
                        break;
                }
                template.NotifyUpdate();
            }
        }
        public void ProcessFaceMediapipe(List<Vector3> faceData, int width, int height)
        {
            var faceModels = m_motionSource.GetBridgesInCategory("FaceLandmark");

            foreach (var baseModel in faceModels.Select(model => model as MPBaseModel))
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
            
            m_solver.Solve(m_solverBuffer, width, height);
            
            var solverModels = m_motionSource.GetBridgesInCategory("FaceSolver");

            foreach (var model in solverModels)
            {
                var solverModel = model as MPSolverModel;
                if (solverModel == null) continue;
                solverModel.SetSolver(m_solver);
                solverModel.Flush();
            }
        }

        public void ProcessPoseMediapipe(List<Vector3> pose)
        {
            var poseModels = m_motionSource.GetBridgesInCategory("PoseLandmark");
            foreach (var model in poseModels.Select(model => model as MPBaseModel))
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
        
        // public void ProcessFaceM4F()
    }
}