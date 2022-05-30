using Assets.Scripts.Core;
using GameJam.Attributes;
using GameJam.Combat;
using GameJam.Movement;
using UnityEngine;
using UnityEngine.UI;
using UX.CharacterInfo;
using TMPro;
using UnityEngine.AI;

namespace GameJam.Control

{
    public class PlayerController : MonoBehaviour
    {
        //references
        Fighter fighter;
        Health health;
        CharacterTransition deathUI;
        NamePicker names;

        [Header("Character Information")]
        [SerializeField] string lastName;
        [SerializeField] string firstName;
        [SerializeField] string crime;
        [SerializeField] int years;
        [Space(5)]

        [Header("Character Modifying Vars")]
        [SerializeField] int minSentence;
        [SerializeField] int maxSentence;
        [Space(5)]
        [SerializeField] TextMeshProUGUI textStashValue;
        public float stashAmount;
        //For music
        [SerializeField] bool isInCombat = false;
        [Header("Combat Vars")]
        [SerializeField] Transform projectileSpawnLocation;
        [SerializeField] GameObject playerProjectile;
        [SerializeField] bool disableIntro;
        [SerializeField] bool hasClearanceCode = false;

        PlayerUIManager playerUI;
        public enum CursorType
        {
            Movement,
            Attack,
            None
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        private void Start()
        {
            GetComponentInChildren<ObjectiveManager>().GenerateObjectives();
        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        void Awake()
        {
            deathUI = FindObjectOfType<CharacterTransition>();
            names = FindObjectOfType<NamePicker>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            health.SetDead(true);
            playerUI = FindObjectOfType<PlayerUIManager>();
            // TODO - Stash == Score? Set
            // textStashValue = GameObject.Find("Stash Value").GetComponent<TextMeshProUGUI>();
            SetNewCharacter(true);
        }

        public void EnableFOB(bool enabled)
        {
            if (enabled)
            {
                SpawnPickup fobSpawner = GameObject.Find("FOB Base Spawn Zone").GetComponent<SpawnPickup>();
                fobSpawner.Respawn();
            } else
            {
                Destroy(GameObject.FindGameObjectWithTag("FOB"));
            }
        }

        public void SetNewCharacter(bool reachedObjective)
        {
            //restore health to full value
            //call animation
            GetComponent<Mover>().CancelAction();
            GetComponent<Fighter>().CancelAction();

            lastName = names.ReadList("lastname");
            firstName = names.ReadList("firstname");
            crime = names.ReadList("crime");
            years = Mathf.RoundToInt(Random.Range(minSentence, maxSentence));
            if (!disableIntro || hasClearanceCode)
            {
                HandlePlayerDeath(reachedObjective);
            }
        }
        /// <summary>
        // protect player from taking further damage and pause gameplay
        // move player to a spawn location
        /// </summary>
        /// <param name="reachedObjective">Whether or not the player reached the FOB with clearance codes</param>
        public void HandlePlayerDeath(bool reachedObjective)
        {
            EnableFOB(false); // Must disable FOB before resetobjective list.. TODO - refactor
            isInCombat = false;
            hasClearanceCode = false;
            health.Respawn();
            FindObjectOfType<Scorekeeper>().AssigneeRunEnd(reachedObjective);
            FindObjectOfType<GameStateUIManager>().AssigneeRunEnd(reachedObjective);
            FindObjectOfType<VoiceManager>().PlayDeathSound(reachedObjective);
            GetComponent<Mover>().ResetToSpawnPosition();

            // STOP ENEMIES FROM TARGETING DURING CUTSCENE
            deathUI.SetPlayerobject(gameObject);

            deathUI.DisplayNewCharacter(reachedObjective, firstName, lastName, crime, years.ToString());

            //StartCoroutine(PlayerDeathWait());
        }

        // public IEnumerator PlayerDeathWait()
        // {
        //     // yield return new WaitForSeconds(4f);
        //     // deathUI.DisplayNewCharacter(reachedObjective, firstName, lastName, crime, years.ToString());
        // }

        public void RestorePlayableCharacter()
        {
            //Reset navmesh

            //Warp to HQ location, to be set later
        }

        // Update is called once per frame
        void Update()
        {
            if (!health.IsAlive()) { return; }
            InteractWithCombat();
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                RaycastHit[] rays = Physics.RaycastAll(GetMouseRay());
                foreach (RaycastHit hit in rays)
                {
                    fighter.SpawnOrbitalLaser(hit.point);
                    return true;
                }
            }
            if (Input.GetMouseButton(1))
            {
                RaycastHit[] rays = Physics.RaycastAll(GetMouseRay());
                foreach (RaycastHit hit in rays)
                {
                    Vector3 position = fighter.GetComponent<Transform>().position;
                    position.z = hit.point.z;
                    position.x = hit.point.x;
                    GetComponent<Animator>().SetTrigger("Attack");
                    Vector3 lookPos = position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
                    fighter.LaunchSpamProjectile(position);
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hitDetails, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"));
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    // TODO - Clean up showing the player they are aiming off navmesh
                    bool affordanceDetailsHit = Physics.Raycast(GetMouseRay(), out RaycastHit affordanceDetails);
                    if (affordanceDetailsHit && affordanceDetails.collider.GetComponent<FadingObject>() != null) { SetCursor(CursorType.None); }
                    Mover mover = GetComponent<Mover>();
                    mover.StartMoveAction(hitDetails.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        public TextMeshProUGUI GetStashText()
        {
            return textStashValue;
        }

        public bool IsInCombat()
        {
            AIController[] enemies = FindObjectsOfType<AIController>();
            foreach(AIController enemy in enemies)
            {
                Fighter enemyFighter = enemy.gameObject.GetComponent<Fighter>();
                if (enemyFighter.GetTarget() != null)
                {
                    isInCombat = true;
                    return true;
                } else
                {
                    isInCombat = false;
                }
            }
            return isInCombat;
        }
        public void LaunchProjectile(Projectile playerProjectile, Transform projectileSpawnLocation, Health target)
        {
            Projectile projectileInstance = Instantiate(playerProjectile, projectileSpawnLocation.position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }

        public void SetClearanceCode(bool clearanceCode)
        {
            hasClearanceCode = clearanceCode;
        }

        public bool HasClearanceCode()
        {
            return hasClearanceCode;
        }
    }
}