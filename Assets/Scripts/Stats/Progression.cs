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

            Debug.Log((int)characterClass);
            Debug.Log(characterClasses[(int)characterClass]);
            return characterClasses[(int)characterClass].GetHealth(level);
        }



        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health=null;

            public float GetHealth(int level)
            {
                return health[level - 1];
            }
        }



    }
}