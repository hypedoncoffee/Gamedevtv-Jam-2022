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

        [SerializeField] Fighter fight;
        [SerializeField] float ignoranceTime;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float timeSinceLastSawPlayer = Mathf.Infinity;
        [SerializeField] bool isSmoked = false;
        [SerializeField] float coyoteTime;
        [SerializeField] float maxCoyoteTime = 1f;
        public void CancelAction()
        {
            GetComponent<Animator>().ResetTrigger("Suspicious");
        }

        public void SuspicionBehavior()
        {
            if(isWatching)
            {
                coyoteTime-=Time.deltaTime;
                if(coyoteTime<0)
                {
                    GetComponent<ActionScheduler>().CancelCurrentAction();
                    GetComponent<Animator>().SetTrigger("Suspicious");
                }
            }
        }

        public bool IsSuspicious()
        {
            return timeSinceLastSawPlayer < suspicionTime;
        }

        public bool IsIgnorant()
        {
            return !isWatching;
        }


        public void IncreaseTimeSinceLastSawPlayer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }
        public void ResetTimeSinceLastSawPlayer()
        {
            if(isWatching)
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
                isSmoked=false;
                isWatching=true;
                ignoranceTime = 0;
            }

        }
        public bool IsSmoked()
        {
            return isSmoked;
        }

        public void Smoke()
        {
            ignoranceTime = 15;
            isSmoked=true;
            isWatching=false;
        }

        void Update()
        {
            if(!isWatching)
            {
                fight.CancelAttack();
                ignoranceTime-=Time.deltaTime;
                //Debug.Log("Time until eyesight returns: "+ignoranceTime);
                if(ignoranceTime<0) Ignorance(false);
            }
        }
    }

}