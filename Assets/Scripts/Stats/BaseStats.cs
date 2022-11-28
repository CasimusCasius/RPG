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
        [SerializeField] bool shouldUseModifiers = false;

        int currentLevel = 0;
        Experience experience;
        private void Start()
        {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();

            if (experience == null) return;
            experience.onExperienceGained += Experience_onExperienceGained;
            
        }
        public int GetLevel()
        {
            if (currentLevel<1)
            {
                currentLevel = CalculateLevel();
            }
           return currentLevel;
        }
        public float GetStat(Stat stat)
        {
            return Mathf.Round((GetBaseStat(stat) + GetAdditiveModifiers(stat))* (1 + GetProcentageModifiers(stat)/100));
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStats(stat, characterClass, GetLevel());
        }
        private float GetProcentageModifiers(Stat stat)
        {

            float sumOfModifieres = 0f;
            if (!shouldUseModifiers) return sumOfModifieres;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                foreach (float modifier in provider.GetProcentageModifiers(stat))
                {
                    sumOfModifieres += modifier;
                }
            }
            return sumOfModifieres;
        }
        private float GetAdditiveModifiers(Stat stat)
        {
            float sumOfModifieres = 0f;
            if (!shouldUseModifiers) return sumOfModifieres;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    sumOfModifieres += modifier;
                }
            }
            return sumOfModifieres;
        }

        private int CalculateLevel()
        {

            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperience();
            int levelsLenght = progression.GetLevels(Stat.ExperirnceToLevelUp, characterClass);
            for (int i = 1; i <= levelsLenght; i++)
            {
                if (currentXP <= progression.GetStats(Stat.ExperirnceToLevelUp, characterClass, i))
                {
                    return i;
                }
            }

            return levelsLenght;
        }
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
