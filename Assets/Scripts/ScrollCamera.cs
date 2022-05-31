using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScrollCamera : MonoBehaviour
{
    CinemachineVirtualCamera maincam;
    float newFov;
    int scrolling = 10;
    int steps = 20;
    [SerializeField] float minFov = 10,maxFov=70;
    [SerializeField] Transform closePoint,midPoint,farPoint;
    [SerializeField] float timeToReach = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis(("Mouse ScrollWheel"))>0)
        {
            scrolling = Mathf.Clamp(scrolling+1,0,steps);
        }
        else if(Input.GetAxis(("Mouse ScrollWheel"))<0)
        {
            scrolling = Mathf.Clamp(scrolling -1,0,steps);
        }
        
    }

    void LateUpdate()
    {
        Quaternion newRot;
        float newFactor = (float)scrolling/(float)steps;
        newRot = Quaternion.Slerp(farPoint.rotation,closePoint.rotation,newFactor);
        newFov = Mathf.Lerp(minFov,maxFov,newFactor);

//            newPos = Vector3.Lerp(midPoint.position,closePoint.position,newFactor);
         
        //transform.rotation = Quaternion.Slerp(transform.rotation,newRot,timeToReach);
    }
}
