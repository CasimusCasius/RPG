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
        [SerializeField] int myLevel;

        private void Start()
        {
           
        }

        public float GetStat(Stat stat) => progression.GetStats(stat, characterClass, GetLevel());
        
        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            
            float currentXP= experience.GetExperience();
            int levelsLenght = progression.GetLevels(Stat.ExperirnceToLevelUp, characterClass);
            for (int i = 1; i <= levelsLenght; i++)
            {
                if (currentXP <= progression.GetStats(Stat.ExperirnceToLevelUp,characterClass,i))
                {
                    myLevel= i; 
                    return i ;
                }
            }

            return levelsLenght;           
        }   
    }
}
