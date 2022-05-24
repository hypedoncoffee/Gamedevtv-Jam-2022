using GameJam.Core;

using GameJam.Stats;
using System;
using UnityEngine;

//only used for respawn call, remove if a more efficient method is possible
using GameJam.Control;

namespace GameJam.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float currentHealth = -1f;
        [SerializeField] bool isAlive = true;
        [SerializeField] bool isPlayer = false;
        [SerializeField] float maxHealth = 100f;
        PlayerUIManager playerUI;
        Animator animator;

        #region RTS Code
        public event Action<float, float> HandleHealthUpdated;
        #endregion
        private void Awake()
        {
            animator = GetComponent<Animator>();
            if(isPlayer) playerUI = FindObjectOfType<PlayerUIManager>();
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
                if(isPlayer) playerUI.SetHealth((int)GetHealth());
            }
        }

        private void Die()
        {
            if (!isAlive) { return; }
            GetComponent<ActionScheduler>().CancelCurrentAction();
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Die");
            isAlive = false;
            if(!isPlayer)
            {
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                //protect player from taking further damage and pause gameplay
                //move player to a spawn location
                GetComponent<PlayerController>().SetNewCharacter(false);
            }
        }

        private void Respawn()
        {
            //Specifically for player: reset health and other stats.
            isAlive=true;
            GetComponent<Collider>().enabled = true;
            currentHealth = maxHealth;
            //TODO: Player character reverts to rigidbody physics.  Should set back to idle with actions enabled.
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
