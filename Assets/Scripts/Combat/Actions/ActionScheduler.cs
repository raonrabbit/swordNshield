using UnityEngine;

namespace SwordNShield.Combat.Actions
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;
        private bool canAction;

        public bool CanAction
        {
            get => canAction;
            set => canAction = value;
        }
        private void Awake()
        {
            canAction = true;
        }

        public void StartAction(IAction action)
        {
            if (!canAction) return;
            if (currentAction == action) return;
            if (currentAction != null) currentAction.Cancel();
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
