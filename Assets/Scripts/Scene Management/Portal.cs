using GameJam.Control;
using GameJam.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace GameJam.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier { MainHub, Graveyard, Tutorial };

        [SerializeField] string sceneToLoad;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 3f;
        [SerializeField] float fadeInTime  = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad == null)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            StopPlayerControl();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal correspondingPortal = GetCorrespondingPortal();
            UpdatePlayer(correspondingPortal);


            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            // Re-enable control but check if the current portal is destroyed (therefore wrong player object race condition)
            GameObject player = GameObject.FindWithTag("Player");
            if (player) { player.GetComponent<PlayerController>().enabled = true; }

            // TODO: Investigate if there is an event to wait for player position updated instead of hardcoding 2f wait
            Destroy(gameObject);
        }

        private static GameObject StopPlayerControl()
        {
            // Stop player in current scene
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            return player;
        }

        private void UpdatePlayer(Portal correspondingPortal)
        {
            Transform portalSpawnPoint = correspondingPortal.spawnPoint;
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portalSpawnPoint.position);
            player.transform.rotation = portalSpawnPoint.rotation;
        }

        private Portal GetCorrespondingPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
                /*
                if (portal.name == scene + " Portal")
                {
                    return portal;
                }
                */
            }
            return null;
        }


    }
}
