using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Atributes

{
    public class Health : MonoBehaviour, ISaveable
    {
        public event Action onHealthChanged;
        public event Action onDead;

        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDeath;

        LazyValue<float> healthPoints;

        bool isDead;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += BaseStats_onLevelUp;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= BaseStats_onLevelUp;
        }
        private void Start()
        {
            healthPoints.ForceInit();
        }
        public void TakeDamage(GameObject damageDealer, float damage)
        {
            
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            //if(gameObject.tag =="Player") 
                
            if (healthPoints.value == 0)
            {
                onDeath.Invoke();
                Die();
                AwardExperienceTo(damageDealer);
            }
            else
            {
                
                takeDamage.Invoke(damage);
            }
            onHealthChanged?.Invoke();

        }

        public float GetProcentage()
        {
            return (healthPoints.value / GetMaxHealthPoints()) * 100;
        }
        private void Die()
        {
            if (isDead) return;
            
            onDead?.Invoke();
            
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public float GetHealthPoints() => healthPoints.value;
        public float GetMaxHealthPoints() => GetComponent<BaseStats>().GetStat(Stat.Health);
        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value == 0)
            {
                Die();
            }
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void BaseStats_onLevelUp()
        {
            healthPoints.value = GetMaxHealthPoints();
            onHealthChanged?.Invoke();
        }

        private void AwardExperienceTo(GameObject damageDealer)
        {
            if (!damageDealer.TryGetComponent<Experience>(out Experience damagerExperience)) return;
            
            damagerExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

    }
}