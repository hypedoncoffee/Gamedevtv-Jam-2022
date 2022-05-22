using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, ProgressionStatFormula>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup(stat, characterClass, level);
            return lookupTable[characterClass][stat].Calculate(level);
        }

        private void BuildLookup(Stat stat, CharacterClass characterClass, int level)
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, ProgressionStatFormula>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stat, ProgressionStatFormula> statDictionary = new Dictionary<Stat, ProgressionStatFormula>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statDictionary[progressionStat.stat] = progressionStat.formula;
                }
                lookupTable[progressionClass.characterClass] = statDictionary;
            }
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public ProgressionStatFormula formula;
        }

        [System.Serializable]
        public class ProgressionStatFormula
        {
            [Range(1, 1000)]
            [SerializeField] float startingValue = 100;
            [Range(0, 1)]
            // TODO: Logarithmic Power Growth
            [SerializeField] float percentageAdded = 0.0f;
            [Range(0, 1000)]
            [SerializeField] float absoluteAdded = 10;

            public float Calculate(int level)
            {
                if (level <= 1) return startingValue;
                float c = Calculate(level - 1);
                return c + (c * percentageAdded) + absoluteAdded;
            }
        }
    }
}