using System.Collections.Generic;
using System.Linq;
using Motion.Data;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace Motion.MotionProcessor
{
    public class MotionProcessor : MonoBehaviour
    {
        public List<MotionTemplateMapper> motionTemplateMapperList = new();

        public void AddMotionTemplateMapper(MotionTemplateMapper motionTemplateMapper)
        {
            motionTemplateMapperList.Add(motionTemplateMapper);
        }
        public void UpdateTemplateByName(string templateName, BridgeItem result)
        {
            foreach (var motionTemplateMapper in motionTemplateMapperList)
            {
                var template = motionTemplateMapper.GetTemplate(templateName);
                if(template == null) continue;

                switch (template)
                {
                    case PointsTemplate pointsTemplate:
                        var pointsItem = (PointsBridgeItem)result;
                        if (pointsItem != null)
                        {
                            UpdatePointsTemplate(pointsTemplate, pointsItem);
                        }
                        break;
                    case AnchorTemplate anchorTemplate:
                        var anchorItem = (AnchorBridgeItem)result;
                        if (anchorItem != null)
                        {
                            UpdateAnchorTemplate(anchorTemplate, anchorItem);
                        }
                        break;
                    case ParametricTemplate parametricTemplate:
                        var parametricItem = (ParametricBridgeItem)result;
                        if (parametricItem != null)
                        {
                            UpdateParametricTemplate(parametricTemplate, parametricItem);
                        }
                        break;
                }
                template.NotifyUpdate();
            }
        }

        private void UpdatePointsTemplate(PointsTemplate template, PointsBridgeItem item)
        {
            var rawPoints = item.rawPoints;
            var visibilities = item.visibilities;
            if (template.points == null || template.points.Length != rawPoints.Length)
            {
                template.points = new Vector3[rawPoints.Length];
            }

            if (template.visibilities == null || template.visibilities.Length != visibilities.Length)
            {
                template.visibilities = new float[visibilities.Length];
            }
            
            Debug.Assert(template.points.Length == template.visibilities.Length);
            
            Enumerable.Range(0, template.points.Length).ToList().ForEach(idx =>
            {
                template.points[idx] = rawPoints[idx];
                template.visibilities[idx] = visibilities[idx];
            });
        }

        private void UpdateAnchorTemplate(AnchorTemplate template, AnchorBridgeItem item)
        {
            template.up = item.up;
            template.lookAt = item.lookAt;
            if(item.position != null) template.position = (Vector3)item.position;
            if(item.scale != null) template.scale = (Vector3)item.scale;
        }

        private void UpdateParametricTemplate(ParametricTemplate template, ParametricBridgeItem item)
        {
            foreach (var (key, value) in item.parametricItems)
            {
                template.SetValue(key, value);
            }
        }
    }
}