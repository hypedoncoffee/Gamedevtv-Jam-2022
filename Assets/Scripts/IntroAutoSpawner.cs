using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAutoSpawner : MonoBehaviour
{
    [SerializeField] GameObject introRobotObj;
    [SerializeField] float spawnDelay=1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBots());
    }

    IEnumerator SpawnBots()
    {
        if(spawnDelay==0) 
        {
            Debug.LogError ("Set a value for spawnDelay or spawners will infinite loop!");
            yield break;
        }
        while(true)
        {
            yield return new WaitForSecondsRealtime(spawnDelay);
            Instantiate(introRobotObj,transform.position,transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
