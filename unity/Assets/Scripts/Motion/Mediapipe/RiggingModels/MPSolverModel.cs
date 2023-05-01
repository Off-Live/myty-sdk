using Motion._3rdParty.MeFaMo;
using Motion.MotionTemplateBridge;

namespace Motion.Mediapipe.RiggingModels
{
    public abstract class MPSolverModel : ParametricBridge
    {
        protected MeFaMoSolver m_solver;

        public void SetSolver(MeFaMoSolver solver)
        {
            m_solver = solver;
        }
    }
}