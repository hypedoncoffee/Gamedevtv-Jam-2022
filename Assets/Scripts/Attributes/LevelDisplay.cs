using GameJam.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace GameJam.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats playerExperience;

        private void Awake()
        {
            playerExperience = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", playerExperience.GetLevel());
        }
    }

}
