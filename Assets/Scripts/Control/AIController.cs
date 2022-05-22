using GameJam.Attributes;
using GameJam.Combat;
using GameJam.Movement;
using UnityEngine;

namespace GameJam.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 4f;
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Mover mover;
        Health health;
        Suspicion suspicion;
        GameObject player;

        #region
        Vector3 guardLocation;
        Quaternion initialLookDirection;
        int currentWaypointIndex = 0;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        #endregion
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            suspicion = GetComponent<Suspicion>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardLocation = transform.position;
            initialLookDirection = transform.rotation;
        }
        // Check every frame if the player is in the chase distance or not
        public void Update()
        {
            if (!health.IsAlive()) { return; }
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (suspicion.IsSuspicious())
            {
                suspicion.SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            suspicion.IncreaseTimeSinceLastSawPlayer();
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            GetComponent<Animator>().ResetTrigger("Suspicious");
            Vector3 nextPosition = guardLocation;

            // If we have a patrol path component assigned to NPC
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint >= waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
            if (Vector3.Distance(transform.position,guardLocation) <= waypointTolerance)
            {
                mover.StartRotateAction(initialLookDirection);
            }
        }
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void AttackBehavior()
        {
            suspicion.ResetTimeSinceLastSawPlayer();
            fighter.Attack(player);
        }


        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
