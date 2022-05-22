using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameJam.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction = null;
        public void StartAction(IAction action)
        {
            if (action == currentAction) return;
            if (currentAction != null)
            {
                currentAction.CancelAction();
            }
            currentAction = action;
        }
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
