using MotionSource._3rdParty.MeFaMo;

namespace MotionSource.Mediapipe.RiggingModels
{
    public abstract class MPSolverModel : MotionTemplateBridge
    {
        protected MeFaMoSolver m_solver;

        public void SetSolver(MeFaMoSolver solver)
        {
            m_solver = solver;
        }
    }
}