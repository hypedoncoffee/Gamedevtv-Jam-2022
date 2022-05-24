using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target!=null)
        {
            transform.RotateAround(target.position,direction,speed*Time.deltaTime);
        }
        else transform.Rotate(direction*speed*Time.deltaTime);
    }
}
