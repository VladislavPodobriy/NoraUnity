using System.Collections;
using UnityEngine;

namespace Nora.NAction
{
    public class NWaitAction : NAction<NWaitAction>
    {
        private readonly float _seconds;
        private Coroutine _waitingRoutine;

        public NWaitAction(float seconds)
        {
            _seconds = seconds;
        }

        public override void Execute(NActionsController controller)
        {
            base.Execute(controller);
            _waitingRoutine = controller.StartCoroutine(Wait());
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(_seconds);
            End();
        }

        public override void Cancel()
        {
            base.Cancel();
            if (_waitingRoutine != null)
                Controller.StopCoroutine(_waitingRoutine);
        }
    }
}