using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Core;

namespace GameJam.Combat
{
    public class Suspicion : MonoBehaviour, IAction
    {
        [SerializeField] bool isWatching;
        void Awake()
        {
            
        }


        [SerializeField] float ignoranceTime;
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
            if(!isWatching)
                timeSinceLastSawPlayer = 0;
        }

        public void Ignorance(bool enabled,float lengthOfIgnorance = 20000f)
        {
            if(enabled)

            {
                isWatching=false;
                ignoranceTime = lengthOfIgnorance;
            }
            else 
            {
                isWatching=true;
                ignoranceTime = 0;
            }

        }

        void Update()
        {
            if(!isWatching)
            {
                if(target!=null) target=null;
                ignoranceTime-=Time.deltaTime;
                Debug.Log("Time until eyesight returns: "+ignoranceTime);
                if(ignoranceTime<0) Ignorance(false);
            }
        }
    }

}