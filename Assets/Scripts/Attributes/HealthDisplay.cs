using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GameJam.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        // TODO - Replace with real UI
        private Health playerHealth;

        private float health;
        [SerializeField] private GameObject healthBarParent = null;
        [SerializeField] private Image healthBarImage = null;

        private void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            playerHealth.HandleHealthUpdated += HandleHealthUpdated;
        }

        private void Update()
        {
            health = playerHealth.GetHealth();
            float healthPercentage = playerHealth.GetPercentage();
            TextMeshPro tmpro = GetComponent<TextMeshPro>();
            string color = "green";

            if (healthPercentage > 75)
            {
                color = "green";
            } else if (healthPercentage <= 75 && healthPercentage > 40) {
                color = "yellow";
             } else if (healthPercentage <= 40) {
                color = "red";
             }
            tmpro.text = String.Format("<color={0}> {1} </color>", color, health);
        }

        private void HandleHealthUpdated(float currentHealth, float maxHealth)
        {
            // TODO : Remove if go for ground health display
            //healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

}
