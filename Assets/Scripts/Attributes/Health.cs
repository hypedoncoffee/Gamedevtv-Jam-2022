using GameJam.Core;
using GameJam.Saving;
using GameJam.Stats;
using System;
using UnityEngine;

namespace GameJam.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        float currentHealth = -1f;
        [SerializeField] bool isAlive = true;
        Animator animator;

        #region RTS Code
        public event Action<float, float> OnHealthUpdated;
        #endregion
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // If health is uninitialized, set to default. This prevents race condition with restore state
            if (currentHealth < 0)
            {
                currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            OnHealthUpdated?.Invoke(currentHealth, GetComponent<BaseStats>().GetStat(Stat.Health));
            if (currentHealth == 0)
            {
                Die(instigator);
                AwardExperience(instigator);
            }
            if (isAlive)
            {
                animator.SetTrigger("Hit");
            }
        }

        private void Die(GameObject instigator)
        {
            if (!isAlive) { return; }
            isAlive = false;
            animator.ResetTrigger("Hit");
            GetComponent<Collider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            animator.SetTrigger("Die");
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            if (instigator != null)
            {
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            }
        }

        public object CaptureState()
        {
            return currentHealth;
        }

        public void RestoreState(object state)
        {
            currentHealth = (float)state;
            if (currentHealth == 0)
            {
                Die(null);
            }
        }

        public float GetPercentage()
        {
            return 100 * (currentHealth / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
    }
}
