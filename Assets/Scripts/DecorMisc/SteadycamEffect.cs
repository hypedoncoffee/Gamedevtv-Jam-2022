using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadycamEffect : MonoBehaviour
{
  [SerializeField] bool steadycam;
  [SerializeField] Transform cameraref;
  [SerializeField] Transform steadycamref;
  [SerializeField] float delayTime;
  [SerializeField] float boundsX=.2f,boundsY=.2f,boundsZ=.2f;
  private Vector3 velocity;
  [SerializeField] private float timeToReach;
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(changePos());
    }

    IEnumerator changePos()
    {
      while(steadycam)
      {

      steadycamref.position = new Vector3 (
        Random.Range(transform.position.x-boundsX,transform.position.x+boundsX),
        Random.Range(transform.position.y-boundsY,transform.position.y+boundsY),
        Random.Range(transform.position.z-boundsZ,transform.position.z+boundsZ));
      yield return new WaitForSeconds(delayTime);
      }
    }

    // Update is called once per frame
    void LateUpdate()
    {
      cameraref.rotation = Quaternion.Slerp(cameraref.rotation,steadycamref.rotation,0.12f);
      cameraref.position = Vector3.SmoothDamp(cameraref.position,steadycamref.position,ref velocity,timeToReach);
    }
}
