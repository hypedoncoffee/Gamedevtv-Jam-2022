using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour
{
    float fovLerp;
    int scrolling = 10;
    [SerializeField] Transform closePoint,midPoint,farPoint;
    private Vector3 velocity;
    float timeToReach = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y>0)
        {
            scrolling = Mathf.Clamp(scrolling+1,0,20);
        }
        else if(Input.mouseScrollDelta.y<0)
        {
            scrolling = Mathf.Clamp(scrolling -1,0,20);
        }
        
    }

    void LateUpdate()
    {
        Vector3 newPos;
        if(scrolling <=10)
            newPos = Vector3.Lerp(farPoint.position,midPoint.position,(float)scrolling/10f);
        else
            newPos = Vector3.Lerp(midPoint.position,closePoint.position,((float)scrolling-10)/10f);
         
        Vector3.SmoothDamp(transform.position,newPos,ref velocity,timeToReach);
    }
}
