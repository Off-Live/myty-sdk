using System.Collections.Generic;
using Motion.Data;

namespace Motion.MotionTemplateBridge
{
    public abstract class ParametricBridge : MotionTemplateBridge
    {
        protected Dictionary<string, float> parameterItems = new();

        public void SetValue(string name, float value)
        {
            parameterItems[name] = value;
        }

        public override BridgeItem CreateItem()
        {
            return new ParametricBridgeItem
            {
                parametricItems = parameterItems
            };
        }
    }
}