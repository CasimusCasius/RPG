using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStats(Stat stat,CharacterClass characterClass, int level)
        {
            foreach (var progressClass in characterClasses)
            {
                if(progressClass.characterClass != characterClass) continue;

                foreach (var progressStat in progressClass.stats)
                {
                    if (progressStat.stat != stat) continue;

                    if (progressStat.levels.Length < level) continue;   

                    return progressStat.levels[level - 1];
                }
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }


    }
}