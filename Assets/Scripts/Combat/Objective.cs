using GameJam.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameJam.Attributes;

namespace GameJam.Combat
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] float timeToHide = 5f;
        [SerializeField] float pickupValue = 100f;
        [SerializeField] string objectiveName = "";

        [SerializeField] bool grantsCode;
        [SerializeField] bool requiresCode;

        public Action<GameObject> objectiveCompleted;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // Not a dropped pickup, living objective. Exit
                Health hasHealth = GetComponent<Health>();
                if (hasHealth) { return; }
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (grantsCode)
                {
                    GrantBuff(playerController);
                } else if (playerController.HasClearanceCode() && requiresCode)
                {
                    playerController.SetNewCharacter(true);
                    return;
                }
                TextMeshProUGUI stashText = playerController.GetStashText();
                playerController.stashAmount = playerController.stashAmount + pickupValue;
                stashText.text = playerController.stashAmount.ToString();
                objectiveCompleted.Invoke(gameObject);
                if (!objectiveName.Equals("FOB"))
                {
                    Destroy(gameObject);
                } else
                {
                    playerController.SetClearanceCode(false);
                }
            }
        }

        private void GrantBuff(PlayerController target)
        {
            target.SetClearanceCode(true);
            target.EnableFOB(true);
        }
        private IEnumerator HideForSeconds(float timeToHide)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(timeToHide);
        }

        private void ShowPickup(bool show)
        {
            GetComponent<Collider>().enabled = show;
            foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
            {
                child.gameObject.SetActive(show);
            }
        }

        public string GetObjectiveName()
        {
            return objectiveName;
        }
    }

}