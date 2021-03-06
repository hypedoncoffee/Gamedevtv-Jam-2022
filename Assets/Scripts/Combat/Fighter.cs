using GameJam.Attributes;
using GameJam.Core;
using GameJam.Movement;

using System;
using UnityEngine;
using UnityEngine.UI;using TMPro;


namespace GameJam.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        //hardcoded final boss stuff
        [Header("Only fill these out for the tank!")]
        [SerializeField] bool isTank;
        [Range(0f,1f)] [SerializeField] float barrelTurnRate;
        [SerializeField] Transform tankBarrel,barrelRefPoint;
        [SerializeField] Vector3 conversionVector;
        //ref point for turning speed
        [Space(5)]
        [Header("Components and vars")]
        [SerializeField] float turnMultiplier;
        [SerializeField] GameObject targetRefPoint;
        //actual script
        [SerializeField] Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        float timeSinceLastAbility = Mathf.Infinity;
        [SerializeField] float timeBetweenAttacks = .5f;
        [SerializeField] int weaponRange;
        [SerializeField] int weaponDamage;
        [Range(0f,1f)] [SerializeField] float turnRate;
        [SerializeField] Projectile projectileWeapon = null;
        [SerializeField] Transform projectileSpawnPoint = null;
        [SerializeField] GameObject orbitalLaserPrefab = null;
        [SerializeField] GameObject orbitalLaserParticles = null;
        [SerializeField] GameObject grenadePrefab = null;
        [SerializeField] GameObject grenadeParticles = null;
        [SerializeField] GameObject smokePrefab = null;
        [SerializeField] GameObject smokeParticles = null;
        [SerializeField] Suspicion sus;
        
        [SerializeField] CharacterTransition deathUI;

        [SerializeField] ParticleSystem fightFX;
        //Ammo management
        [SerializeField] bool isPlayer;
        bool reloading = false;
        [SerializeField] Button reloadButton;
        [SerializeField] Slider reloadSlider;
        [SerializeField] AudioClip reloadSound;
        float reloadTime;
        [SerializeField] float maxReloadTime = 2f;
        [SerializeField] int maxAmmo=30,currentAmmo=30;

        [SerializeField] float abilityCooldown = 1.5f;

        Animator animator = null;



        void Update()
        {
            if(target==null&&fightFX!=null) fightFX.Stop();
            // Time.deltaTime = Time since the last time update is called
            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastAbility += Time.deltaTime;
            if (target == null || !target.IsAlive())  { target = null; return; }

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
            if(!sus || !sus.IsIgnorant())
            if(targetRefPoint != null)
            {
                targetRefPoint.transform.LookAt(target.transform);
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRefPoint.transform.rotation,turnRate*turnMultiplier*Time.deltaTime);
            }
            if(isTank)
            {
                tankBarrel.rotation = Quaternion.Slerp(tankBarrel.rotation,barrelRefPoint.transform.rotation,barrelTurnRate*turnMultiplier*Time.deltaTime);
                barrelRefPoint.transform.LookAt(target.transform,conversionVector);
            }
            else transform.LookAt(target.transform);
            
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        void Awake()
        {
            deathUI = FindObjectOfType<CharacterTransition>();
            sus = GetComponent<Suspicion>();
          //  fightFX = GetComponent<ParticleSystem>();
        }

        private void TriggerAttack()
        {
            if(currentAmmo > 0)
            {
                currentAmmo = currentAmmo - 1;
                if(reloadSlider != null) reloadSlider.value = currentAmmo;
                if (target == null) { return; }
                if (projectileWeapon != null && projectileSpawnPoint != null)
                {
                    GetComponent<Animator>().ResetTrigger("StopAttack");
                    GetComponent<Animator>().SetTrigger("Attack");
                    LaunchProjectile();
                } else
                {
                    target.TakeDamage(weaponDamage);
                }
            }
            else if(isPlayer)
            {
                reloadButton.gameObject.SetActive(true);
            }
            else if (reloading)
            {
                if(reloadTime > 0)
                    reloadTime -= Time.deltaTime;
                else 
                {
                    Reload();
                }
            }
            else
            {
                reloadTime = maxReloadTime;
                reloading = true;
                //StartCoroutine(EnemyReload());
            }
        }

        // IEnumerator EnemyReload()
        // {
        //     yield return new WaitForSeconds(reloadTime);
        //     Reload();
        // }

        public void Reload()
        {
            currentAmmo = maxAmmo;
            if(reloadButton != null) reloadButton.gameObject.SetActive(false);
            if(reloadSlider != null) reloadSlider.value = maxAmmo;
            GetComponent<AudioSource>().PlayOneShot(reloadSound);
            reloading = false;
        }


        public void LaunchProjectile()
        {
            currentAmmo--;
            Projectile projectileInstance;
            if(!isTank)
            {
                projectileInstance = Instantiate(projectileWeapon, projectileSpawnPoint.position, Quaternion.identity);
                projectileInstance.SetTarget(target);
            }
            else
            {
                projectileInstance = Instantiate(projectileWeapon, projectileSpawnPoint.position, tankBarrel.transform.rotation);            
                projectileInstance.StraightFire(target); 
            } 
        }

        public void LaunchSpamProjectile(Vector3 position)
        {
            if(currentAmmo > 0)
            {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                currentAmmo--;
                Projectile projectileInstance = Instantiate(projectileWeapon, projectileSpawnPoint.position, Quaternion.identity);
                if (isPlayer)
                {
                    projectileInstance.SetShotByEnemy(false);
                } else
                {
                    projectileInstance.SetShotByEnemy(true);
                }
                projectileInstance.FireInDirection(position);
                timeSinceLastAttack = 0;
                if(reloadSlider != null) reloadSlider.value = currentAmmo;

                TriggerStopAttack();
            }
            }
            else if(reloadButton!=null) reloadButton.gameObject.SetActive(true);
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) { return false; }
            if (deathUI.transition) { return false;}
            Health targetToTest = target.GetComponent<Health>();
            if (!targetToTest.IsAlive())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            return targetToTest != null && targetToTest.IsAlive();
        }

        internal void SpawnOrbitalLaser(Vector3 point)
        {
            {
                Instantiate(orbitalLaserPrefab, point, Quaternion.identity);
                Instantiate(orbitalLaserParticles, point, Quaternion.identity);
                timeSinceLastAbility = 0;
            }
        }
         internal void SpawnGrenade(Vector3 point)
        {
            {
                Instantiate(grenadePrefab, point, Quaternion.identity);
                Instantiate(grenadeParticles, point, Quaternion.identity);
                timeSinceLastAbility = 0;
            }
        }
         internal void SpawnSmokeGrenade(Vector3 point)
        {
            {
                Instantiate(smokePrefab, point, Quaternion.identity);
                Instantiate(smokeParticles, point, Quaternion.identity);
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
