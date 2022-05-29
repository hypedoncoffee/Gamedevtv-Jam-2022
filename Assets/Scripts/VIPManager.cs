using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UX.CharacterInfo;
using GameJam.Control;
public class VIPManager : MonoBehaviour
{
    [Tooltip("This controls the number of VIPs for VIP 0.")]
    [SerializeField] int startingGuardCount=2;

    [Tooltip("This controls the number of VIPs killed before guard count increases.")]
    [SerializeField] int guardIncreaseRate=2;

    [Tooltip("This controls the number of extra guards per increase.")]
    [SerializeField] int guardIncreaseCount=2;

    [SerializeField] GameObject vipPrefab,guardPrefab,finalVIPPrefab;
    [SerializeField] int vipCount;
    int nextVIP;
    [Space(5)]
    [SerializeField] info[] vips;
    [SerializeField]GameObject[] vipSpawners;
    [SerializeField]GameObject finalSpawn;

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
    }
    
    void Start()
    {
        int nextGuards = startingGuardCount;
        vips = new info[vipCount];

        for(int i = 0; i < vips.Length ;i ++)
        {
            if(i%guardIncreaseRate==0) nextGuards+=guardIncreaseCount;
            vips[i].lastName = names.ReadList("lastname");
            vips[i].firstName = names.ReadList("firstname");
            vips[i].numberOfGuards = nextGuards; 
        }    
        //Threw this in Start for testing, call SpawnVIP() under any other circumstances
        //StartCoroutine(FirstVIPSpawn());
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
        if(nextVIP < vips.Length-1)
        {
            GameObject newVIP = Instantiate(vipPrefab,vipSpawners[nextVIP].transform.position,Quaternion.identity);
            newVIP.GetComponent<AIController>().PassVIPInfo(vips[nextVIP].lastName+", "+vips[nextVIP].firstName,vips[nextVIP].id);
            nextVIP++;
        }
        else
        {
            Instantiate(finalVIPPrefab,finalSpawn.transform.position,Quaternion.identity);
        }
        //tankCode
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
    }

    public string VIPInfo(int vipID)
    {
        return vips[vipID].lastName + ", " + vips[vipID].firstName;
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
