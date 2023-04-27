using System;
using System.Collections.Generic;
using System.Linq;
using MotionSource._3rdParty.MeFaMo;
using MYTYKit.MotionTemplates;
using UnityEngine;

namespace MotionSource.Mediapipe.RiggingModels
{
    [Serializable]
    internal class BSItem
    {
        public string name;
        public float value;
    }
    public class MPBlendshape : MPSolverModel
    {   
        [SerializeField] List<BSItem> blendShape = new();
        protected override void Process()
        {
            if (m_solver == null) return;
            blendShape.Clear();
            foreach (var key in Enum.GetValues(typeof(MeFaMoConfig.FaceBlendShape)))
            {
                var keyString = key + "";
                var keyEnum = (MeFaMoConfig.FaceBlendShape)key;
                keyString = char.ToLower(keyString.First()) + keyString.Substring(1);
                if (m_solver.blendShape.ContainsKey(keyEnum))
                {
                    blendShape.Add(new BSItem
                    {
                        name = keyString,
                        value = m_solver.blendShape[keyEnum]
                    });
                }
            }
            m_solver = null;

        }

        protected override void UpdateValue()
        {
            blendShape.ForEach(item => SetValue(item.name,item.value));
        }
    }
}