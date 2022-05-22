using GameJam.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameJam.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0,10000)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            // TODO : Change for losing level
            if (newLevel != currentLevel)
            {
                currentLevel = newLevel;
            }
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }
        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentExperience = experience.GetExperienceValue();
            // TODO - Don't calculate level every time
            for (int level = 1; level < int.MaxValue; level++)
            {
                float requiredXpForNextLevel = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (requiredXpForNextLevel > currentExperience)
                {
                    return level;
                }
            }
            return 1;
        }
    }

}