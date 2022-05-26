using GameJam.Attributes;
using UnityEngine;


namespace GameJam.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 25;
        [SerializeField] int weaponDamage = 10;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject[] hitEffect = null;
        [SerializeField] int maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] int lifeAfterImpact = 0;

        bool dumbMissile = false;
        Health target = null;

        void Update()
        {
            if (target == null && dumbMissile == false) return;
            if (isHoming && target.IsAlive())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target)
        {
            this.target = target;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, maxLifeTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (dumbMissile)
            {
                Health hasHealth = other.GetComponent<Health>();
                if (other.CompareTag("Player") || !hasHealth) { return; }
                HandleHitEffects(other.transform.position);
                if (hasHealth != null)
                {
                    hasHealth.TakeDamage(weaponDamage);
                }
                DestroyOnHits();
            } else
            {
                NormalProjectileBehavior(other);
            }
        }

        private void NormalProjectileBehavior(Collider other)
        {
            Health collisionObjectHealth = other.gameObject.GetComponent<Health>();
            if (collisionObjectHealth != target) return;
            if (collisionObjectHealth == null) return;
            HandleHitEffects(collisionObjectHealth.transform.position);
            if (collisionObjectHealth.IsAlive())
            {
                collisionObjectHealth.TakeDamage(weaponDamage);
            }
            DestroyOnHits();
        }

        private void DestroyOnHits()
        {
            foreach (GameObject toDoDestroy in destroyOnHit)
            {
                Destroy(toDoDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }

        private void HandleHitEffects(Vector3 hitPosition)
        {
            if (hitEffect != null)
            {
                foreach (GameObject effect in hitEffect)
                {
                    Instantiate(effect, hitPosition, transform.rotation);
                }
            }
        }

        private Vector3 GetAimLocation()
        {
            // TODO - This should become an aimpoint on the enemy, and potentially a different aim point depending on hit math
            // Block, Critical, etc.
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) { return target.transform.position; };
            float sixthHeight = targetCapsule.height / 6;
            return target.transform.position + Vector3.up * (targetCapsule.height / 2 + UnityEngine.Random.Range(-sixthHeight, sixthHeight));
        }


        public void FireInDirection(Vector3 position)
        {
            dumbMissile = true;
            transform.LookAt(position);
            Destroy(gameObject, maxLifeTime);
        }
    }
}