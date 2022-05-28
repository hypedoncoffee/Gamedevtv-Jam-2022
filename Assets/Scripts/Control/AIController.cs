using GameJam.Attributes;
using GameJam.Combat;
using GameJam.Movement;
using UnityEngine;
using TMPro;
using UX.CharacterInfo;
namespace GameJam.Control
{
    public class AIController : MonoBehaviour
    {
        //name
        [SerializeField] string lastName,firstName;
        [SerializeField] bool isVIP = false;
        [SerializeField] int vipID = 9999;
        [Space(5)]

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
        [SerializeField] TMP_Text charNameUI;
        [SerializeField] ParticleSystem alertFX;

        //Audio stuff
        [SerializeField] float timeSinceLastCallout;
        [SerializeField] PatrolVoice voices;
        [SerializeField] float minTimeBetweenCallouts = 15;

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
            if(charNameUI!=null&&!isVIP)SetName();
        }


        void SetName()
        {
            NamePicker names = FindObjectOfType<NamePicker>();
            lastName = names.ReadList("lastname");
            firstName = names.ReadList("firstname");
            charNameUI.text = lastName+", "+firstName;
        }

        public void PassVIPInfo(string newname, int id)
        {
            charNameUI.text = newname;    
            vipID = id;
        }

        public void KillVIP()
        {
            //presumably called from Health?
            FindObjectOfType<VIPManager>().KillVIP(vipID);
            //Instantiate Briefcase here
            //Instnatiate(briefcaseObj,transform.position,transform.rotation);
            //Destroy(this.gameObject,10f);
            //Animation stuff
        }

        // Check every frame if the player is in the chase distance or not
        public void Update()
        {
            //Audio hook for idle banter
            timeSinceLastCallout += Time.deltaTime;


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
            // GetComponent<Animator>().ResetTrigger("Suspicious");
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
            if(timeSinceLastCallout > minTimeBetweenCallouts)
            {
                timeSinceLastCallout = 0;
                if(voices!=null) voices.Alert(true);
            }
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, (transform.position.z + chaseDistance)));
        }
    }
}
