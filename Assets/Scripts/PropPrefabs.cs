using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPrefabs : MonoBehaviour
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
        SpawnProp();
    }



    public void SpawnProp()
    {
        startTime = Time.realtimeSinceStartup;
        GameObject spawnBuilding = Instantiate(
            buildings.SpawnBuilding(halfSize,doubleSize),
            transform.position,transform.rotation);
    
        spawnBuilding.transform.localScale = new Vector3(Random.Range(scaleMult-scaleMinusX,scaleMult+scalePlusX),Random.Range(scaleMult-scaleMinusY,scaleMult+scalePlusY),Random.Range(scaleMult-scaleMinusZ,scaleMult+scalePlusZ));
        //Destroy(this.gameObject);
        endTime = Time.realtimeSinceStartup;
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
            SpawnProp();
        }
    }
    */
}
