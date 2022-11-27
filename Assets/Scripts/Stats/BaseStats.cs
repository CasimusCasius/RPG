using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] Progression progression = null;
        

        int currentLevel = 0;
        Experience experience;
        private void Start()
        {
            currentLevel = CalculateLevel();
            experience = GetComponent<Experience>();
            experience.onExperienceGained += Experience_onExperienceGained;
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            
            float currentXP= experience.GetExperience();
            int levelsLenght = progression.GetLevels(Stat.ExperirnceToLevelUp, characterClass);
            for (int i = 1; i <= levelsLenght; i++)
            {
                if (currentXP <= progression.GetStats(Stat.ExperirnceToLevelUp,characterClass,i))
                { 
                    return i ;
                }
            }

            return levelsLenght;           
        }
        public int GetLevel()
        {
            if (currentLevel<1)
            {
                currentLevel = CalculateLevel();
            }
           return currentLevel;
        }
        public float GetStat(Stat stat) => progression.GetStats(stat, characterClass, GetLevel());

        private void Experience_onExperienceGained()
        {
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel= newLevel;
                print("Level Up !!!");
            }
        }
    }

}
