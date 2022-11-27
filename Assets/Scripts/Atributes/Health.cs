using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Atributes

{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthPoints = -1f;

        bool isDead;
        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }     
        }

        public void TakeDamage(GameObject damageDealer, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperienceTo(damageDealer);
            }
        }

        public float GetProcentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }
        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void AwardExperienceTo(GameObject damageDealer)
        {
           

            if (!damageDealer.TryGetComponent<Experience>(out Experience damagerExperience)) return;
            
            damagerExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

    }
}