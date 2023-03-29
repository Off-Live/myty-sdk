using System.Collections.Generic;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource
{
    public abstract class MotionTemplateBridge : MonoBehaviour
    {
        protected List<MotionTemplate> templateList = new();

        public void AddMotionTemplate(MotionTemplate template)
        {
            templateList.Add(template);
        }

        public void ClearMotionTemplate()
        {
            templateList.Clear();
        }

        public void Flush()
        {
            Process();
            UpdateTemplate();
        }

        protected abstract void UpdateTemplate();
        protected abstract void Process();
    }
}