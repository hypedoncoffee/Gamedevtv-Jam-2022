using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPoolManager : MonoBehaviour
{
    [SerializeField] GameObject[] fullSizeBuilding,halfSizeBuilding,doubleSizeBuilding;
    [SerializeField] GameObject[] fullSizeProp;
    [SerializeField] float timeForBuilding = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAllBuildings());
        
    }

    IEnumerator SpawnAllBuildings()
    {
        BuildingPrefabs[] spawners = FindObjectsOfType<BuildingPrefabs>();
        foreach (BuildingPrefabs spawner in spawners)
        {
            if(gameObject.active)
            {
            yield return new WaitForSecondsRealtime(timeForBuilding/(float)spawners.Length);
            spawner.SpawnBuilding();
            }




        }
        SpawnAllProps();
    }

    IEnumerator SpawnAllProps()
    {
        PropPrefabs[] spawners = FindObjectsOfType<PropPrefabs>();
        foreach (PropPrefabs spawner in spawners)
        {
            yield return new WaitForSecondsRealtime(20f/(float)spawners.Length);
            spawner.SpawnProp();

        }
    }

    public GameObject SpawnBuilding (bool halfSize=false,bool doubleSize=false)
    {
        int rng;
        if(halfSize)
        {
            rng = Random.Range(0,halfSizeBuilding.Length-1);
            return fullSizeBuilding[rng];

        }
        else if(doubleSize)
        {
            rng = Random.Range(0,doubleSizeBuilding.Length-1);
            return fullSizeBuilding[rng];

        }
        else
        {
            rng = Random.Range(0,fullSizeBuilding.Length-1);
            return fullSizeBuilding[rng];
        }
    }

    public GameObject SpawnProp (bool halfSize=false,bool doubleSize=false)
    {
        int rng;
        if(halfSize)
        {
            rng = Random.Range(0,halfSizeBuilding.Length-1);
            return fullSizeBuilding[rng];

        }
        else if(doubleSize)
        {
            rng = Random.Range(0,doubleSizeBuilding.Length-1);
            return fullSizeBuilding[rng];

        }
        else
        {
            rng = Random.Range(0,fullSizeProp.Length-1);
            return fullSizeProp[rng];
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
