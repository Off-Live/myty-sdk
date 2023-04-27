using System.Collections.Generic;

namespace Motion.MotionTemplateBridge
{
    public abstract class ParametricBridge : MotionTemplateBridge
    {
        protected Dictionary<string, float> parameterItems = new();

        public void SetValue(string name, float value)
        {
            parameterItems[name] = value;
        }
    }
}