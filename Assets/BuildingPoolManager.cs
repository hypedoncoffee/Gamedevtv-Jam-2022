using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPoolManager : MonoBehaviour
{
    [SerializeField] GameObject[] fullSizeBuilding,halfSizeBuilding,doubleSizeBuilding;
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
            yield return new WaitForSeconds(20f/(float)spawners.Length);
            spawner.SpawnBuilding();

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

    // Update is called once per frame
    void Update()
    {
        
    }

}
