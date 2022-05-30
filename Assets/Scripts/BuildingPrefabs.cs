using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrefabs : MonoBehaviour
{
    [SerializeField] float startTime, endTime;
    [SerializeField] float scaleMinusX,scaleMinusY,scaleMinusZ,scalePlusX,scalePlusY,scalePlusZ;
    [SerializeField] bool halfSize,doubleSize;
    BuildingPoolManager buildings;
    [SerializeField] float scaleMult = 1;

    GameObject building;
    // Start is called before the first frame update
    void Start()
    {

        buildings= FindObjectOfType<BuildingPoolManager>();
       // SpawnBuilding();
    }



    public void SpawnBuilding()
    {
        startTime = Time.realtimeSinceStartup;
        GameObject spawnBuilding = Instantiate(
            buildings.SpawnBuilding(halfSize,doubleSize),
            transform.position,transform.rotation,this.transform);
    
        spawnBuilding.transform.localScale = new Vector3(Random.Range(scaleMult-scaleMinusX,scaleMult+scalePlusX),Random.Range(scaleMult-scaleMinusY,scaleMult+scalePlusY),Random.Range(scaleMult-scaleMinusZ,scaleMult+scalePlusZ));
        //Destroy(this.gameObject);
        endTime = Time.realtimeSinceStartup;
        GetObjectsInBoxCollider(GetComponent<BoxCollider>());
    }

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
             while (t.parent != null && t.GetComponent<BuildingPrefabs>() == null) t = t.parent;
             if (t.GetComponent<BuildingPrefabs>() != null 
                 && t != collider.transform 
                 && !objectsInBox.Contains(t)) 
                 objectsInBox.Add(t);
         }
         for (int i = 0; i < objectsInBox.Count; i++) objectsInBox[i].gameObject.SetActive(false);
     }
    // Update is called once per frame
    void Update()
    {
        
    }

    // if this doesn't work check dist or place on player

/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnBuilding();
        }
    }
*/
}
