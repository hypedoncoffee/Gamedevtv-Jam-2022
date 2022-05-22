using GameJam.Attributes;
using UnityEngine;

namespace GameJam.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController mainhandOverrideController = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 3f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransformHand(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator);
        }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                // Set name of instantiated weapon
                GameObject weapon = Instantiate(equippedPrefab, GetTransformHand(rightHand, leftHand));
                weapon.name = weaponName;
            }
            // Casting to check if it is of type
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (mainhandOverrideController != null)
            {
                animator.runtimeAnimatorController = mainhandOverrideController;
            } else if (overrideController != null)
            {
                // we have an existing override controller
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform weaponToDestroy = rightHand.Find(weaponName);
            if (weaponToDestroy == null)
            {
                weaponToDestroy = leftHand.Find(weaponName);
            }
            if (weaponToDestroy == null) return;

            weaponToDestroy.name = "Destroying";
            Destroy(weaponToDestroy.gameObject);
        }

        private Transform GetTransformHand(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }
    }
}
