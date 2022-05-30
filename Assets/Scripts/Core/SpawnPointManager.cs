using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPoints;

    GameObject lastSpawnPoint = null;

    public GameObject GetNewSpawnPoint()
    {
        if (spawnPoints.Count == 0) { spawnPoints = new List<GameObject>() { GameObject.Find("Player Spawn Point") }; };
        foreach (var spawnPoint in spawnPoints)
        {
            if(lastSpawnPoint == null)
            {
                lastSpawnPoint = spawnPoint;
                return spawnPoint;
            } else
            {
                if(lastSpawnPoint != spawnPoint)
                {
                    return spawnPoint;
                }
            }
        }
        return spawnPoints[0];
    }
}
