using GameJam.Core;

using GameJam.Stats;
using System;
using UnityEngine;

namespace GameJam.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float currentHealth = -1f;
        [SerializeField] bool isAlive = true;
        [SerializeField] float maxHealth = 100f;
        Animator animator;

        #region RTS Code
        public event Action<float, float> HandleHealthUpdated;
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
                currentHealth = maxHealth;
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            HandleHealthUpdated?.Invoke(currentHealth, maxHealth);
            if (currentHealth == 0)
            {
                Die();
            }
            if (isAlive)
            {
                animator.SetTrigger("Hit");
            }
        }

        private void Die()
        {
            if (!isAlive) { return; }
            isAlive = false;
            animator.ResetTrigger("Hit");
            GetComponent<Collider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            animator.SetTrigger("Die");
        }

        public float GetPercentage()
        {
            return 100 * (currentHealth / maxHealth);
        }

        public float GetHealth()
        {
            return currentHealth;
        }
    }
}
