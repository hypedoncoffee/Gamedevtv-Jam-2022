using GameJam.Core;

using GameJam.Stats;
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
                    isObjective.pickedUp.Invoke(isObjective.gameObject);
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
