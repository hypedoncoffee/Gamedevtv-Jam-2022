using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameJam.Control;
public class OpenSesame : MonoBehaviour
{
    bool unlocked;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Unlocked()
    {
        unlocked= true;
    }
    public void OpenDoor()
    {
        GetComponent<Animator>().SetTrigger("opendoor");
        GetComponent<NavMeshObstacle>().enabled=false;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(unlocked)
                OpenDoor();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
