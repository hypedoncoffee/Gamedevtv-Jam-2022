using GameJam.Attributes;
using GameJam.Core;
using GameJam.Movement;
using GameJam.Saving;
using System;
using UnityEngine;
namespace GameJam.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] float timeBetweenAttacks = 1.21f;
        [SerializeField] Transform righthandTransform = null;
        [SerializeField] Transform lefthandTransform = null;
        [SerializeField] Weapon defaultMainhandWeapon = null;
        [SerializeField] Weapon defaultOffhandWeapon = null;
        [SerializeField] Weapon currentMainhandWeapon = null;
        [SerializeField] Weapon currentOffhandWeapon = null;
        // TODO: Look at implementing UUID resource loading

        Animator animator = null;
        void Awake()
        {
            if (currentMainhandWeapon == null)
            {
                EquipMainhandWeapon(defaultMainhandWeapon);
            }
            if (currentOffhandWeapon == null)
            {
                EquipOffhandWeapon(defaultOffhandWeapon);
            }
        }

        void Update()
        {
            // Time.deltaTime = Time since the last time update is called
            timeSinceLastAttack += Time.deltaTime;
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
                return Vector3.Distance(transform.position, target.transform.position) < GetLongestWeaponRange();
            }
        }

        private float GetLongestWeaponRange()
        {
            float bestRange = 0;
            if (currentMainhandWeapon.name != "Unarmed" && currentOffhandWeapon.name == "Unarmed")
            {
                bestRange = currentMainhandWeapon.GetWeaponRange();
            } else if (currentOffhandWeapon.name != "Unarmed" && currentMainhandWeapon.name == "Unarmed")
            {
                bestRange = currentOffhandWeapon.GetWeaponRange();
            } else
            {
                bestRange = currentMainhandWeapon.GetWeaponRange();
            }
            
            return bestRange;
        }

        public void EquipMainhandWeapon(Weapon weapon)
        {
            animator = GetComponent<Animator>();
            currentMainhandWeapon = weapon;
            weapon.Spawn(righthandTransform, lefthandTransform, animator);
        }
        public void EquipOffhandWeapon(Weapon weapon)
        {
            animator = GetComponent<Animator>();
            currentOffhandWeapon = weapon;
            weapon.Spawn(righthandTransform, lefthandTransform, animator);
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
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }
            if (currentMainhandWeapon.HasProjectile() || currentOffhandWeapon.HasProjectile())
            {
                if (currentOffhandWeapon.name == "Unarmed") {
                    currentMainhandWeapon.LaunchProjectile(lefthandTransform, righthandTransform, target, gameObject);
                } else {
                    currentOffhandWeapon.LaunchProjectile(lefthandTransform, righthandTransform, target, gameObject);
                }
                //currentMainhandWeapon.LaunchProjectile(lefthandTransform, righthandTransform, target);
            } else
            {
                target.TakeDamage(gameObject, currentMainhandWeapon.GetWeaponDamage());
            }
        }

        void ReleaseArrow()
        {
            Hit();
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
        [Serializable]
        struct FighterSaveData
        {
            public string currentMainhandWeaponName;
            public string currentOffhandWeaponName;
        }
        public object CaptureState()
        {
            FighterSaveData saveData = new FighterSaveData();
            if (currentMainhandWeapon == null)
            {
                saveData.currentMainhandWeaponName = "Unarmed";
            } else
            {
                saveData.currentMainhandWeaponName = currentMainhandWeapon.name;
            }
            if (currentOffhandWeapon == null)
            {
                saveData.currentOffhandWeaponName = "Unarmed";
            } else
            {
                saveData.currentOffhandWeaponName = currentOffhandWeapon.name;
            }
            return saveData;
        }

        public void RestoreState(object state)
        {
            FighterSaveData saveData = (FighterSaveData)state;
            Weapon mainhandWeapon = Resources.Load<Weapon>(saveData.currentMainhandWeaponName);
            Weapon offhandWeapon = Resources.Load<Weapon>(saveData.currentOffhandWeaponName);
            EquipMainhandWeapon(mainhandWeapon);
            //EquipOffhandWeapon(offhandWeapon);
        }

        public Health GetTarget()
        {
            return target;
        }
    }
}
