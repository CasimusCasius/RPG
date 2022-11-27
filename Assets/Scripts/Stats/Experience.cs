using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{


    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;

        //public delegate void ExperienceGainedDelegate(); (Action domyœlny delegat)
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        public float GetExperience() => experiencePoints;

        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
