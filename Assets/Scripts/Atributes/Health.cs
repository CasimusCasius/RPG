using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Atributes

{
    public class Health : MonoBehaviour, ISaveable
    {
        public event Action onHealthChanged;

        float healthPoints = -1f;

        bool isDead;
        private void Awake()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetMaxHealthPoints();

            }
        }
        private void Start()
        {
           
            GetComponent<BaseStats>().onLevelUp += BaseStats_onLevelUp;
            
        }

        public void TakeDamage(GameObject damageDealer, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(gameObject.tag =="Player") onHealthChanged();
            if (healthPoints == 0)
            {
                Die();
                AwardExperienceTo(damageDealer);
            }

        }

        public float GetProcentage()
        {
            return (healthPoints / GetMaxHealthPoints()) * 100;
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

        public float GetHealthPoints() => healthPoints;
        public float GetMaxHealthPoints() => GetComponent<BaseStats>().GetStat(Stat.Health);
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

        private void BaseStats_onLevelUp()
        {
            healthPoints = GetMaxHealthPoints();
            onHealthChanged();
        }

        private void AwardExperienceTo(GameObject damageDealer)
        {
            if (!damageDealer.TryGetComponent<Experience>(out Experience damagerExperience)) return;
            
            damagerExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

    }
}