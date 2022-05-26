using GameJam.Attributes;
using GameJam.Core;
using GameJam.Movement;

using System;
using UnityEngine;
namespace GameJam.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        float timeSinceLastAbility = Mathf.Infinity;
        [SerializeField] float timeBetweenAttacks = .5f;
        [SerializeField] int weaponRange;
        [SerializeField] int weaponDamage;
        [SerializeField] Projectile projectileWeapon = null;
        [SerializeField] Transform projectileSpawnPoint = null;
        [SerializeField] GameObject orbitalLaserPrefab = null;
        [SerializeField] GameObject orbitalLaserParticles = null;

        [SerializeField] float abilityCooldown = 1.5f;

        Animator animator = null;

        void Update()
        {
            // Time.deltaTime = Time since the last time update is called
            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastAbility += Time.deltaTime;
            if (target == null || !target.IsAlive()) { target = null; return; }

            if (!GetIsInRange())
            {
                // Not in range
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                // In range
                GetComponent<Mover>().CancelAction();
                AttackBehaviour();
            }

            bool GetIsInRange()
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                return distance < GetRange();
            }
        }

        private float GetRange()
        {
            return weaponRange;
        }


        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
            if (target == null) { return; }
            if (projectileWeapon != null && projectileSpawnPoint != null)
            {
                LaunchProjectile();
            } else
            {
                target.TakeDamage(weaponDamage);
            }
        }


        public void LaunchProjectile()
        {
            Projectile projectileInstance = Instantiate(projectileWeapon, projectileSpawnPoint.position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }

        public void LaunchSpamProjectile(Vector3 position)
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                Projectile projectileInstance = Instantiate(projectileWeapon, projectileSpawnPoint.position, Quaternion.identity);
                projectileInstance.FireInDirection(position);
                timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) { return false; }
            Health targetToTest = target.GetComponent<Health>();
            if (!targetToTest.IsAlive())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            return targetToTest != null && targetToTest.IsAlive();
        }

        internal void SpawnOrbitalLaser(Vector3 point)
        {
            if (timeSinceLastAbility > abilityCooldown)
            {
                Instantiate(orbitalLaserPrefab, point, Quaternion.identity);
                Instantiate(orbitalLaserParticles, point, Quaternion.identity);
                timeSinceLastAbility = 0;
            }
        }

        public void AttackSpam()
        {
            if (projectileWeapon != null && projectileSpawnPoint != null)
            {
                LaunchProjectile();
            }
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            target = null;
        }
        public void CancelAction()
        {
            CancelAttack();
            TriggerStopAttack();
            GetComponent<Mover>().CancelAction();
        }

        private void TriggerStopAttack()
        {
            GetComponent<Animator>().SetTrigger("StopAttack");
            GetComponent<Animator>().ResetTrigger("Attack");
        }

        public Health GetTarget()
        {
            return target;
        }
    }
}
