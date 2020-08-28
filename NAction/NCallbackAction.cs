using System;

namespace Nora.NAction
{
    public class NCallbackAction : NAction<NCallbackAction>
    {
        private readonly Action _callback;

        public NCallbackAction(Action callback) : base()
        {
            _callback = callback;
        }

        public override void Execute(NActionsController controller)
        {
            base.Execute(controller);

            _callback();

            End();
        }
    }
}