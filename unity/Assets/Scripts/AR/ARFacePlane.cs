using MYTYKit.MotionTemplates;
using UnityEngine;

namespace AR
{
    public abstract class ARFacePlane : MonoBehaviour
    {
        public MotionTemplateMapper motionTemplateMapper;
        
        public MeshRenderer arBounds;

        public Transform facePlane;

        void Update()
        {
            UpdateFromMotionTemplate();
        }
        
        protected abstract void UpdateFromMotionTemplate();
    }
}