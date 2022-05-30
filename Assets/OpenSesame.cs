using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameJam.Control;
public class OpenSesame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            if(other.GetComponent<PlayerController>().HasClearanceCode())
                OpenDoor();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
