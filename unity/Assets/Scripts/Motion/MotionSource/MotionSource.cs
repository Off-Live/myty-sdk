using System;
using System.Collections.Generic;
using UnityEngine;

namespace Motion.MotionSource
{
    [Serializable]
    public class MotionCategory
    {
        public string name;
        public List<MotionTemplateBridge.MotionTemplateBridge> bridges;
    }

    [Serializable]
    public class MTBridgeItem
    {
        public string name;
        public MotionTemplateBridge.MotionTemplateBridge templateBridge;
    }

    public abstract class MotionSource : MonoBehaviour
    {
        [SerializeField] List<MotionCategory> motionCategories = new();
        [SerializeField] List<MTBridgeItem> templateBridgeMap = new();
        
        public MotionProcessor.MotionProcessor motionProcessor;

        public List<string> GetCategoryList()
        {
            List<string> ret = new();
            foreach (var category in motionCategories)
            {
                ret.Add(category.name);
            }

            return ret;
        }

        public List<MotionTemplateBridge.MotionTemplateBridge> GetBridgesInCategory(string categoryName)
        {
            foreach (var category in motionCategories)
            {
                if (category.name == categoryName)
                {
                    return category.bridges;
                }
            }

            return null;
        }

        public void AddMotionTemplateBridge(string categoryName, MotionTemplateBridge.MotionTemplateBridge bridge)
        {
            var index = -1;
            for (int i = 0; i < motionCategories.Count; i++)
            {
                if (categoryName == motionCategories[i].name)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                var newItem = new MotionCategory()
                {
                    name = categoryName,
                    bridges = new()
                };
                newItem.bridges.Add(bridge);
                motionCategories.Add(newItem);
            }
            else
            {
                var list = motionCategories[index].bridges;
                list.Add(bridge);
            }
        }

        public void Clear()
        {
            motionCategories.Clear();
        }

        public void Process(string result)
        {
            ConvertCapturedResult(result);
            templateBridgeMap.ForEach(item =>
                motionProcessor.UpdateTemplateByName(item.name, item.templateBridge.CreateItem())
            );
        }
        
        protected abstract void ConvertCapturedResult(string result);
        
        public abstract float GetWorldPointWidth();
        
        public abstract float GetWorldPointHeight();
    }
}