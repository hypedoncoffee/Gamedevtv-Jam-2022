using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Combat
{
    public class SpawnPickup : MonoBehaviour
    {
        [SerializeField] float pickupSpawnRadius = 10f;
        [SerializeField] GameObject spawnedItem;
        [SerializeField] bool spawnImmediately = false;
        private bool spawned = false;

        private void Awake()
        {
            if (spawnImmediately)
            {
                Spawn();
            }
        }

        public void Spawn()
        {
            if (!spawned)
            {
                float randomX = transform.position.x + UnityEngine.Random.Range(-pickupSpawnRadius / 2, pickupSpawnRadius + 2);
                float randomZ = transform.position.z + UnityEngine.Random.Range(-pickupSpawnRadius / 2, pickupSpawnRadius + 2);
                Vector3 randomPosition = new Vector3(
                    randomX,
                    transform.position.y,
                    randomZ
                );
                spawned = true;
                GameObject newItem = Instantiate(spawnedItem, randomPosition, transform.rotation);
                ObjectiveManager objectiveManager = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponentInChildren<ObjectiveManager>();
                newItem.GetComponent<Objective>().objectiveCompleted += objectiveManager.handleObjectivePickup;
                objectiveManager.AddObjective(newItem);
            }
        }

        public void Respawn()
        {
            float randomX = transform.position.x + UnityEngine.Random.Range(-pickupSpawnRadius / 2, pickupSpawnRadius + 2);
            float randomZ = transform.position.z + UnityEngine.Random.Range(-pickupSpawnRadius / 2, pickupSpawnRadius + 2);
            Vector3 randomPosition = new Vector3(
                randomX,
                transform.position.y,
                randomZ
            );
            spawned = true;
            GameObject newItem = Instantiate(spawnedItem, randomPosition, transform.rotation);
            ObjectiveManager objectiveManager = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponentInChildren<ObjectiveManager>();
            newItem.GetComponent<Objective>().objectiveCompleted += objectiveManager.handleObjectivePickup;
            objectiveManager.AddObjective(newItem);
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, pickupSpawnRadius);
        }
    }

}