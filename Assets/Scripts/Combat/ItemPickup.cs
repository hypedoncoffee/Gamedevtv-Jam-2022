using GameJam.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam.Combat
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] float timeToHide = 5f;
        [SerializeField] float pickupValue = 100f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                Text stashText = playerController.GetStashText();
                playerController.stashAmount = playerController.stashAmount + pickupValue;
                stashText.text = playerController.stashAmount.ToString();
                StartCoroutine(HideForSeconds(timeToHide));
            }
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
    }

}