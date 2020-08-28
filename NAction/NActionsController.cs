using UnityEngine;

namespace Nora.NAction
{
    public class NActionsController: MonoBehaviour
    {
        public NAction CurrentAction;

        public void Execute<T>(NAction<T> action) where T: NAction<T>
        {
            Cancel();
            CurrentAction = action;
            action.Execute(this);
        }

        public void Cancel()
        {
            if (CurrentAction != null && CurrentAction.GetStatus() == ActionStatus.Active)
                CurrentAction.Cancel();
        }
    }
}
