using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] int[] health=null;
            
        }

    }
}