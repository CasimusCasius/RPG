using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (var progressClass in characterClasses)
            {
                if(progressClass.GetCharacterClass() == characterClass)
                {
                    return progressClass.GetHealth(level);
                }
            }
            
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health;


            public CharacterClass GetCharacterClass() => characterClass;
            public float GetHealth(int level) => health[level - 1];
            
        }



    }
}