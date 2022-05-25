using GameJam.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GameJam.Combat
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] float timeToHide = 5f;
        [SerializeField] float pickupValue = 100f;

        public Action<GameObject> pickedUp;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                TextMeshProUGUI stashText = playerController.GetStashText();
                playerController.stashAmount = playerController.stashAmount + pickupValue;
                stashText.text = playerController.stashAmount.ToString();
                StartCoroutine(HideForSeconds(timeToHide));
                pickedUp.Invoke(this.gameObject);
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