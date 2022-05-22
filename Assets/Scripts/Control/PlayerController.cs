using GameJam.Attributes;
using GameJam.Combat;
using GameJam.Movement;
using UnityEngine;
namespace GameJam.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            if (!health.IsAlive()) { return; }
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] rays = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in rays)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!fighter.CanAttack(target.gameObject)) { continue; }

                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hitDetails);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    Mover mover = GetComponent<Mover>();
                    mover.StartMoveAction(hitDetails.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}