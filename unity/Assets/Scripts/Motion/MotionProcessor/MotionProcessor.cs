using System;
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

        CalibrationItem m_faceCalibration = new CalibrationItem();
        public void SetSyncedBlinkScale(float value) => m_faceCalibration.syncedBlinkScale = value;
        public void SetBlinkScale(float value) => m_faceCalibration.blinkScale = value;
        public void SetPupilScale(float value) => m_faceCalibration.pupilScale = value;
        public void SetEyebrowScale(float value) => m_faceCalibration.eyebrowScale = value;
        public void SetMouthXScale(float value) => m_faceCalibration.mouthXScale = value;
        public void SetMouthYScale(float value) => m_faceCalibration.mouthYScale = value;

        public void AddMotionTemplateMapper(MotionTemplateMapper motionTemplateMapper)
        {
            motionTemplateMapperList.Add(motionTemplateMapper);
        }

        public void UpdateTemplateByName(string templateName, BridgeItem result)
        {
            foreach (var motionTemplateMapper in motionTemplateMapperList)
            {
                var template = motionTemplateMapper.GetTemplate(templateName);
                if (template == null) continue;

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
            if (item.position != null) template.position = (Vector3)item.position;
            if (item.scale != null) template.scale = (Vector3)item.scale;
        }

        private void UpdateParametricTemplate(ParametricTemplate template, ParametricBridgeItem item)
        {
            var items = item.parametricItems;

            ApplyCalibration(items);

            foreach (var (key, value) in items)
            {
                template.SetValue(key, value);
            }
        }

        private void ApplyCalibration(Dictionary<string, float> items)
        {
            if (items.ContainsKey("leftEye") && items.ContainsKey("rightEye"))
            {
                var leftEye = items["leftEye"];
                var rightEye = items["rightEye"];
                if (Mathf.Abs(leftEye - rightEye) <= m_faceCalibration.syncedBlinkScale)
                {
                    var maxEye = Mathf.Max(leftEye, rightEye);
                    items["leftEye"] = maxEye;
                    items["rightEye"] = maxEye;
                }
            }

            ApplyCalibrationEach(items, "leftEye",
                f => m_faceCalibration.blinkScale * (f - 1.0f) + 1.0f);
            ApplyCalibrationEach(items, "rightEye",
                f => m_faceCalibration.blinkScale * (f - 1.0f) + 1.0f);
            ApplyCalibrationEach(items, "leftEyeBrow",
                f => m_faceCalibration.eyebrowScale * (f - 0.5f) + 0.5f);
            ApplyCalibrationEach(items, "rightEyeBrow",
                f => m_faceCalibration.eyebrowScale * (f - 0.5f) + 0.5f);
            ApplyCalibrationEach(items, "leftPupil",
                f => m_faceCalibration.pupilScale * f);
            ApplyCalibrationEach(items, "rightPupil",
                f => m_faceCalibration.pupilScale * f);
            ApplyCalibrationEach(items, "mouthX",
                f => m_faceCalibration.mouthXScale * (f - 0.4f) + 0.4f);
            ApplyCalibrationEach(items, "mouthY",
                f => m_faceCalibration.mouthYScale * f);
        }

        private void ApplyCalibrationEach(Dictionary<string, float> items, string key, Func<float, float> updateFunc)
        {
            if (!items.ContainsKey(key)) return;
            var value = items[key];
            items[key] = updateFunc(value);
        }
    }
}