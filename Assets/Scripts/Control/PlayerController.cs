using Assets.Scripts.Core;
using GameJam.Attributes;
using GameJam.Combat;
using GameJam.Movement;
using UnityEngine;
using UnityEngine.UI;
using UX.CharacterInfo;
using TMPro;
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

        [SerializeField] CursorMapping[] cursorMappings = null;
        void Start()
        {
            deathUI = FindObjectOfType<CharacterTransition>();
            names = FindObjectOfType<NamePicker>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            textStashValue = GameObject.Find("Stash Value").GetComponent<TextMeshProUGUI>();
            SetNewCharacter(true);
        }

        public void SetNewCharacter(bool successful)
        {
            //restore health to full value
            //call animation
            lastName= names.ReadList("lastname");
            firstName = names.ReadList("firstname");
            crime = names.ReadList("crime");
            years = Mathf.RoundToInt(Random.Range(minSentence,maxSentence));
            deathUI.DisplayNewCharacter(successful,firstName,lastName,crime,years.ToString());
        }


        public void RestorePlayableCharacter()
        {
            //Reset navmesh

            //Warp to HQ location, to be set later
        }

        // Update is called once per frame
        void Update()
        {
            if (!health.IsAlive()) { return; }
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] rays = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in rays)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!fighter.CanAttack(target.gameObject)) { continue; }
                SetCursor(CursorType.Attack);
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
    }
}