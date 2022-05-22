using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Combat
{
    public class ItemPickupZone : MonoBehaviour
    {
        [SerializeField] float pickupSpawnRadius = 10f;
        [SerializeField] GameObject spawnedItem;

        private bool spawned = false;

        private void Start()
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
                Instantiate(spawnedItem, randomPosition, transform.rotation);
            }
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, pickupSpawnRadius);
        }
    }

}