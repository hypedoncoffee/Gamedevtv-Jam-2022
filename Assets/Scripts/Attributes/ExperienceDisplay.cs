using System;
using UnityEngine;
using UnityEngine.UI;
namespace GameJam.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience playerExperience;
        private void Awake()
        {
            playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0}", playerExperience.GetExperienceValue());
        }
    }

}
