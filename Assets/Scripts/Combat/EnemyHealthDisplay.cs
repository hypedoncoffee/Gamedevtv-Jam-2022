using GameJam.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace GameJam.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        // TODO - Replace with real UI
        private Health targetHealth;

        private Fighter playerFighter;
        private void Awake()
        {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            targetHealth = playerFighter.GetTarget();
        }

        private void Update()
        {
            targetHealth = playerFighter.GetTarget();
            if (targetHealth == null)
            {
                GetComponent<Text>().text = "N/A";
            } else
            {
                GetComponent<Text>().text = String.Format("{0:0}%", targetHealth.GetPercentage());
            }
        }
    }

}
