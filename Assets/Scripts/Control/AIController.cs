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
        [SerializeField] float waypointTolerance = 3f;
        [SerializeField] float waypointDwellTime = 1f;
        [SerializeField] float patrolSpeedFraction = 1f;

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

        [SerializeField] bool isPatrolling;

        #region
        Vector3 guardLocation;
        Quaternion initialLookDirection;
        int currentWaypointIndex = 0;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        #endregion        
        ParticleSystem fightFX;
        [Range(0,100)]int voiceChanceThreshold=40;

        private void Start()
        {
//            fightFX = GetComponent<ParticleSystem>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            suspicion = GetComponent<Suspicion>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardLocation = transform.position;
            initialLookDirection = transform.rotation;
            if(charNameUI!=null&&!isVIP)SetName();
            if(isPatrolling && patrolPath==null) SetPatrolPathRandom();
        }

        public void LogName()
        {
        Scorekeeper score = FindObjectOfType<Scorekeeper>();
            if(!isVIP)
            score.LogName(lastName,firstName,"Aide to Treasonous Individuals",160.ToString());
            else
            score.LogName(lastName,firstName,"High Treason", 700.ToString()); 
        }


        void SetName()
        {
            NamePicker names = FindObjectOfType<NamePicker>();
            if(names!=null)
            {

            lastName = names.ReadList("lastname");
            firstName = names.ReadList("firstname");
            charNameUI.text = lastName+", "+firstName;
            charNameUI.text = charNameUI.text.Replace("\r","");
            }
        }

        public void SetPatrolPath(GameObject newPatrolPath)
        {
            patrolPath = newPatrolPath.GetComponent<PatrolPath>();
        }
        public void SetPatrolPathRandom()
        {   
            PatrolPath[] paths = FindObjectsOfType<PatrolPath>(); 
            patrolPath = paths[Random.Range(0,paths.Length-1)];
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
            if (!player || !player.activeInHierarchy)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                return;
            }
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
            Vector3 nextPosition;
            if (patrolPath)
            {
                nextPosition = GetCurrentWaypoint();
            } else
            {
                nextPosition = guardLocation;
            }

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
            if(fightFX!=null)fightFX.Play();
            if(timeSinceLastCallout > minTimeBetweenCallouts)
            {
                timeSinceLastCallout = 0;
                //5.04: Reduce voice line likelihood
                int rng = Random.Range(0,100);
                if(rng < voiceChanceThreshold)
                { 
                    if(voices!=null&&!suspicion.IsIgnorant()) voices.Alert(true);
                    if(voices!=null&&!suspicion.IsSmoked()) voices.HeardSomething();
                }
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
