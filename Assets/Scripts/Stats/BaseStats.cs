using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action onLevelUp;
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpVFXPrefab= null;

        int currentLevel = 0;
        Experience experience;
        private void Start()
        {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();

            if (experience == null) return;
            experience.onExperienceGained += Experience_onExperienceGained;
            
        }

        public int CalculateLevel()
        {
            
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
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpVFXPrefab == null) return;

            Instantiate(levelUpVFXPrefab,transform);
        }
    }

}
