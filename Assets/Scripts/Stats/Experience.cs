
using System;
using UnityEngine;

namespace GameJam.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints;

        //public delegate void ExperienceGainedDelegate();
        // Note Action is the same as a void delegate
        public event Action onExperienceGained;


        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        public float GetExperienceValue()
        {
            return experiencePoints;
        }
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
