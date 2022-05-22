using GameJam.Attributes;
using UnityEngine;

namespace GameJam.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] float weaponDamage = 3f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject[] hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;


        Health target = null;
        GameObject instigator = null;

        void Update()
        {
            if (target == null) return;
            if (isHoming && target.IsAlive())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator)
        {
            this.target = target;
            this.instigator = instigator;
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, maxLifeTime);
        }

        void OnTriggerEnter(Collider other)
        {
            Health collisionObjectHealth = other.gameObject.GetComponent<Health>();
            if (collisionObjectHealth != target) return;
            if (collisionObjectHealth == null) return;
            if (hitEffect != null)
            {
                foreach (GameObject effect in hitEffect)
                {
                    Instantiate(effect, GetAimLocation(), transform.rotation);
                }
            }
            if (collisionObjectHealth.IsAlive())
            {
                collisionObjectHealth.TakeDamage(instigator, weaponDamage);
            }
            foreach (GameObject toDoDestroy in destroyOnHit)
            {
                Destroy(toDoDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
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
    }
}