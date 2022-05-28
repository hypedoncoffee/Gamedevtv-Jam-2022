using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UX.CharacterInfo;

public class VIPManager : MonoBehaviour
{

    [SerializeField] int vipCount;
    [Space(5)]
    [SerializeField] info[] vips;
    GameObject[] vipSpawners;

    NamePicker names;
    
    [System.Serializable]public struct info
    {
        public string lastName;
        public string firstName;
        public bool alive;
    }
    // Start is called before the first frame update
    
    void Awake()
    {
        names = FindObjectOfType<NamePicker>();
    }
    
    void Start()
    {
        vips = new info[vipCount];
        for(int i = 0; i < vips.Length ;i ++)
        {
            vips[i].lastName = names.ReadList("lastname");
            vips[i].firstName = names.ReadList("firstname"); 
        }    
    }

    public string vipName(int vipIndex)
    {
        return vips[vipIndex].lastName +", "+vips[vipIndex].firstName;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
