using System;
using UnityEngine;
using UnityEngine.UI;
namespace GameJam.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        // TODO - Replace with real UI
        private Health playerHealth;

        private Health health;
        private GameObject healthBarParent = null;
        private Image healthBarImage = null;

        private void Awake()
        {
            //health = GetComponent<Health>();
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            //health.OnHealthUpdated += HandleHealthUpdated;
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}%", playerHealth.GetPercentage());
        }

        private void OnDestroy()
        {
            //health.OnHealthUpdated -= HandleHealthUpdated;
        }

        private void OnMouseEnter()
        {
            //healthBarParent.SetActive(true);
        }

        private void OnMouseExit()
        {
            //healthBarParent.SetActive(false);
        }

        private void HandleHealthUpdated(float currentHealth, float maxHealth)
        {
            //healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

}
