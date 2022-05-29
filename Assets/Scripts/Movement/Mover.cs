using GameJam.Core;
using UnityEngine;
using UnityEngine.AI;

using GameJam.Attributes;

namespace GameJam.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 15f;

        NavMeshAgent navMeshAgent;
        Health health;
        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = health.IsAlive();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            // When we are get velocity, this is a global velocity in the world space.
            // We need to know in a local velocity that does not care about the global values relative to the world space
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z; // Forward direction
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
        public void StartRotateAction(Quaternion rotation)
        {
            if (IsAtDestination())
            {
                GetComponent<ActionScheduler>().StartAction(this);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 5);
            }
        }

        public void StopMovement()
        {
            navMeshAgent.isStopped = true;
        }
        public bool IsAtDestination()
        {
            return navMeshAgent.remainingDistance == 0;
        }

        public void CancelAction()
        {
            StopMovement();
        }
        public void ResetToSpawnPosition()
        {
            Vector3 spawnPosition = GameObject.Find("Player Spawn Point").GetComponent<Transform>().position;
            navMeshAgent.updatePosition = false;
            transform.position = spawnPosition;
            navMeshAgent.updatePosition = true;
        }
    }
}

