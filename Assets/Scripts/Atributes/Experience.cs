using RPG.Saving;
using UnityEngine;

namespace RPG.Atributes
{


    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;



        public void GainExperience(float experience)
        {
            experiencePoints += experience;
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
