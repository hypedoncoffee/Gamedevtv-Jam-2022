using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UX.CharacterInfo;
using GameJam.Control;
using GameJam.Movement;
using UnityEngine.AI;
using GameJam.Combat;

public class VIPManager : MonoBehaviour
{
    [Tooltip("This controls the number of VIPs for VIP 0.")]
    [SerializeField] int startingGuardCount=3;

    [Tooltip("This controls the number of VIPs killed before guard count increases.")]
    [SerializeField] int guardIncreaseRate=1;

    [Tooltip("This controls the number of extra guards per increase.")]
    [SerializeField] int guardIncreaseCount=3;

    [SerializeField] GameObject vipPrefab,guardPrefab,finalVIPPrefab;
    [SerializeField] int vipCount;
    int nextVIP;
    [Space(5)]
    [SerializeField] info[] vips;
    [SerializeField] GameObject[] vipSpawners;
    [SerializeField] GameObject finalSpawn;
    [SerializeField] GameStateUIManager gamehud;
    [SerializeField] private float spawnMoveRange = 0f;
    [SerializeField] private float spawnOffsetRange = 3f;

    NamePicker names;
    
    
    [System.Serializable]
    public struct info
    {
        public int id;
        public string lastName;
        public string firstName;
        public int numberOfGuards;
        public bool defeated;
    }
    
    void Awake()
    {
        names = FindObjectOfType<NamePicker>();
        vipSpawners = GameObject.FindGameObjectsWithTag("VIPSpawner");
        Difficulty diff = FindObjectOfType<Difficulty>();
        if (diff) vipCount = diff.NumberOfVips;
        InitializeVips();
    }
    

    private void InitializeVips()
    {
        int nextGuards = startingGuardCount;
        vips = new info[vipCount];

        for (int i = 0; i < vips.Length; i++)
        {
            nextGuards += guardIncreaseCount;
            vips[i].lastName = names.ReadList("lastname");
            vips[i].firstName = names.ReadList("firstname");
            vips[i].numberOfGuards = nextGuards;
        }
    }

    IEnumerator FirstVIPSpawn()
    {
        //just for testing lol
    yield return new WaitForSeconds(2f);
        SpawnVIP();
        SpawnVIP();
        SpawnVIP();
        SpawnVIP();
        SpawnVIP();
        SpawnVIP();
    }

    public string vipName(int vipIndex)
    {
        return vips[vipIndex].lastName +", "+vips[vipIndex].firstName;
    } 

    public void SpawnVIP()
    {
        if(nextVIP < vips.Length-2)
        {
            GameObject newVIP = Instantiate(vipPrefab,vipSpawners[nextVIP].transform.position,Quaternion.identity);
            for (int guardsToSpawn = 0; guardsToSpawn < vips[nextVIP].numberOfGuards; guardsToSpawn++)
            {
                Vector3 spawnOffset = (Random.insideUnitSphere * spawnOffsetRange);
                spawnOffset.y = vipSpawners[nextVIP].transform.position.y;
                vipSpawners[nextVIP].transform.position = vipSpawners[nextVIP].transform.position + spawnOffset;

                NavMeshHit hit;
                GameObject guard;
                if (NavMesh.SamplePosition(vipSpawners[nextVIP].transform.position, out hit, 5f, NavMesh.AllAreas)) 
                {
                    guard = Instantiate(guardPrefab, hit.position, Quaternion.identity);
                    Vector3 walkOffset = Random.insideUnitSphere * spawnMoveRange;
                    walkOffset.y = vipSpawners[nextVIP].transform.position.y;

                    Mover guardMove = guard.GetComponent<Mover>();
                    AIController guardAi = guard.GetComponent<AIController>();
                    GameObject patrolPath = GameObject.Find("Random FOB Walk");
                    guardAi.SetPatrolPath(patrolPath);

                    //guardMove.StartMoveAction(vipSpawners[nextVIP].transform.position + walkOffset, 1);
                }
            }
            newVIP.GetComponent<AIController>().PassVIPInfo(vips[nextVIP].lastName+", "+vips[nextVIP].firstName,vips[nextVIP].id);
            nextVIP++;
        }
        else if (nextVIP==vips.Length-1)
        {
            FindObjectOfType<OpenSesame>().Unlocked();
            gamehud.FinalObjective();
            Instantiate(finalVIPPrefab,finalSpawn.transform.position,Quaternion.identity);
        }
        else FindObjectOfType<Scorekeeper>().GameEnd();
        gamehud.UpdateHitList();
        //tankCode
    }

    public int totalVIPS()
    {
        return vipCount;
    }

    public void KillVIP(int killID)
    {
        for (int i = 0; i < vips.Length;i++) 
        {
            if(vips[i].id == killID)
            {
                vips[i].defeated = true;
            }
        }
        FindObjectOfType<Scorekeeper>().KillVIP();
    }

    public string VIPInfo(int vipID)
    {
        return vips[vipID].lastName + ", " + vips[vipID].firstName;
    }
    public bool isVipDead(int vipID)
    {
        return vips[vipID].defeated;
    }

    public int nextLivingVIP()
    {
        for (int i = 0; i < vips.Length;i++) 
        {
            if(!vips[i].defeated)
            {
            return vips[i].id;
            }
            
        }
        return 9999;
    }
}
