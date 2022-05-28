using GameJam.Core;

using System;
using UnityEngine;

//only used for respawn call, remove if a more efficient method is possible
using GameJam.Control;
using TMPro;
using GameJam.Combat;

namespace GameJam.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int currentHealth = -1;
        [SerializeField] bool isAlive = true;
        [SerializeField] bool isPlayer = false;
        [SerializeField] int maxHealth = 100;
        PlayerUIManager playerUI;
        Animator animator;

        public event Action<float, float> HandleHealthUpdated;
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
            UpdateHealthUI();
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        private void UpdateHealthUI()
        {
            TextMeshPro tmpro = GetComponentInChildren<TextMeshPro>();
            if (tmpro)
            {
                tmpro.text = String.Format("<color={0}> {1} </color>", nextColor(currentHealth), currentHealth);
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);
            if (currentHealth == 0)
            {
                Die();
            }
            if (isAlive)
            {
                animator.SetTrigger("Hit");
                if (isPlayer)
                {
                    playerUI.SetHealth((int)GetHealth());
                } else
                {
                    UpdateHealthUI();
                }
            }
        }
        /// <summary>
        /// Handles if this health component is attached to an entity that has a drop. If so, it spawns the drop as a pickup.
        /// If the entity is also an objective, invoke that objectives pickudUp method.
        /// Cancels the entities current action with the action scheduler, and sets it to dead for future health calculations.
        /// </summary>
        private void Die()
        {
            if (!isAlive) { return; }
            SpawnPickup hasItemDrop = GetComponent<SpawnPickup>();
            if (hasItemDrop)
            {
                hasItemDrop.Spawn();
                Objective isObjective = GetComponent<Objective>();
                if (isObjective)
                {
                    if (isObjective.gameObject)
                    {
                        isObjective.objectiveCompleted.Invoke(isObjective.gameObject);
                    }
                }
                AIController aiController = GetComponent<AIController>();
                if (aiController)
                {
                    aiController.KillVIP();
                }
            }
            GetComponent<ActionScheduler>().CancelCurrentAction();
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Die");
            isAlive = false;
            if(!isPlayer)
            {
                // TODO : Animation
                Destroy(gameObject);
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                GetComponent<PlayerController>().HandlePlayerDeath(false);
            }
        }
        /// <summary>
        /// Specifically for player: reset health and other stats.
        /// </summary>
        public void Respawn()
        {
            isAlive=true;
            GetComponent<Collider>().enabled = true;
            currentHealth = maxHealth;
            //TODO: Player character reverts to rigidbody physics.  Should set back to idle with actions enabled.
        }

        public float GetPercentage()
        {
            return 100 * (currentHealth / maxHealth);
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public string nextColor(int nextHealth)
        {
            if (nextHealth > 67)
                return "green";

            else if (nextHealth > 33 && nextHealth <= 67)
                return "yellow";

            else if (nextHealth <= 33)
                return "red";
            else
                return "red";

        }
    }
}
