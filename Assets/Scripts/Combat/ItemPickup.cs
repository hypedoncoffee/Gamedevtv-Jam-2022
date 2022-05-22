using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Combat
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float timeToHide = 5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(HideForSeconds(timeToHide));
            }
        }

        private IEnumerator HideForSeconds(float timeToHide)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(timeToHide);
            ShowPickup(true);
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