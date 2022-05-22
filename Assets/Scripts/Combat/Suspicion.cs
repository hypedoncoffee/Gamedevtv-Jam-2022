using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Core;

namespace GameJam.Combat
{
    public class Suspicion : MonoBehaviour, IAction
    {
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float timeSinceLastSawPlayer = Mathf.Infinity;
        public void CancelAction()
        {
            GetComponent<Animator>().ResetTrigger("Suspicious");
        }

        public void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("Suspicious");
        }

        public bool IsSuspicious()
        {
            return timeSinceLastSawPlayer < suspicionTime;
        }

        public void IncreaseTimeSinceLastSawPlayer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }
        public void ResetTimeSinceLastSawPlayer()
        {
            timeSinceLastSawPlayer = 0;
        }
    }

}