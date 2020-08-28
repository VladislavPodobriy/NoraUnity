using System;

namespace Nora.NAction
{
    public abstract class NAction
    {
        public abstract void Execute(NActionsController controller);
        public abstract void Cancel();
        public abstract void End();
        public abstract void SetStatus(ActionStatus status);
        public abstract ActionStatus GetStatus();
    }

    public class NAction<T> : NAction where T : NAction<T>
    {
        public NAction BindedAction;

        private ActionStatus _status;

        private Action _onStart;
        private Action _onEnd;
        private Action _onCancel;

        protected NActionsController Controller;

        public NAction()
        {
            _status = ActionStatus.Created;
        }

        public override void Execute(NActionsController controller)
        {
            Controller = controller;
            _status = ActionStatus.Active;
            _onStart?.Invoke();
        }

        public override void Cancel()
        {
            _status = ActionStatus.Canceled;
            BindedAction?.Cancel();
            _onCancel?.Invoke();
        }

        public override void End()
        {
            _status = ActionStatus.Ended;
            BindedAction?.Execute(Controller);
            _onEnd?.Invoke();
        }

        public override void SetStatus(ActionStatus status)
        {
            _status = status;
        }

        public override ActionStatus GetStatus()
        {
            return _status;
        }

        public T OnStart(Action onStart) 
        {
            _onStart = onStart;
            return (T)this;
        }

        public T OnEnd(Action onEnd)
        {
            _onEnd = onEnd;
            return (T)this;
        }

        public T OnCancel(Action onCancel)
        {
            _onCancel = onCancel;
            return (T)this;
        }

        public virtual TR Bind<TR>(NAction<TR> action) where TR: NAction<TR>
        {
            BindedAction = action;
            return (TR)action;
        }

        public virtual NCallbackAction Bind(Action callback)
        {
            var action = new NCallbackAction(callback);
            BindedAction = action;
            return action;
        }
    }
}
