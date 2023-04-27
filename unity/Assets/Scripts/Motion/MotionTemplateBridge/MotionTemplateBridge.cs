using UnityEngine;

namespace Motion.MotionTemplateBridge
{
    public abstract class MotionTemplateBridge : MonoBehaviour
    {
        public void Flush()
        {
            Process();
            UpdateValue();
        }

        protected abstract void UpdateValue();
        protected abstract void Process();
    }
}