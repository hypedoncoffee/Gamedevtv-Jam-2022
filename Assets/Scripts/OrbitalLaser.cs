using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Attributes;

public class OrbitalLaser : MonoBehaviour
{

    [SerializeField] float laserDamage = 50f;
    [SerializeField] float laserTime = 5f;
    // Start is called before the first frame update
    void Awake()
    {
        //Detect all in sphere
        GetObjectsInBoxCollider(GetComponent<BoxCollider>());
        Destroy(this.gameObject,laserTime);
    }

//https://answers.unity.com/questions/1695742/getting-a-list-of-gameobjects-in-a-collider.html
      private void GetObjectsInBoxCollider(BoxCollider collider)
     {
         Collider[] colliders = Physics.OverlapBox(
             center:collider.transform.position + (collider.transform.rotation * collider.center), 
             halfExtents:Vector3.Scale(collider.size * 0.5f, collider.transform.lossyScale), 
             orientation:collider.transform.rotation, 
             layerMask:~0);
         List<Transform> objectsInBox = new List<Transform>();
         foreach (var c in colliders)
         {
             Transform t = c.transform;
             while (t.parent != null && t.GetComponent<Health>() == null) t = t.parent;
             if (t.GetComponent<Health>() != null 
                 && t != collider.transform 
                 && !objectsInBox.Contains(t)) 
                 objectsInBox.Add(t);
         }
         for (int i = 0; i < objectsInBox.Count; i++) objectsInBox[i].GetComponent<Health>().TakeDamage(laserDamage);
     }


    // Update is called once per frame
    void Update()
    {
        
    }
}
