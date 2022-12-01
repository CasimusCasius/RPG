using GameDevTV.Utils;
using RPG.Atributes;
using RPG.Core;
using RPG.Movment;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction, ISaveable,IModifierProvider
    {
        
        [SerializeField] float timeBeetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
       

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        
        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
            
            
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            
            if (target.IsDead())
            {
                Cancel();
                return;
            }
            if (!IsInRange())
            {

                GetComponent<Mover>().MoveTo(target.transform.position, 1);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBeetweenAttacks)
            {
                // This will trigger Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        public bool CanAttack(GameObject target)
        {
            if (target == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(target.transform.position,currentWeaponConfig.GetAttackRange())  
                && Vector3.Distance(transform.position,target.transform.position) > currentWeaponConfig.GetAttackRange()) { return false; }
            Health targetToTest = target.GetComponent<Health>();
            return (targetToTest != null && !targetToTest.IsDead());

        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
            GetComponent<Mover>()?.Cancel();
        }
        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();

             return weapon?.Spawn(rightHandTransform, leftHandTransform, animator); 
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
            
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }
        public IEnumerable<float> GetProcentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponProcentageBonus();
            }
        }
        public Health GetTarget() => target;

        // AnimationEvent
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectiles())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject,damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        // AnimationEvent
        void Shoot() { 
            Hit();
        }

        private bool IsInRange()
        {
           
            return Vector3.Distance(target.transform.position, transform.position) <= currentWeaponConfig.GetAttackRange();
        }
        private void TriggerStopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        
        //Save
        public object CaptureState()
        {
           
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
           
            WeaponConfig weapon = Resources.Load<WeaponConfig>((string)state);
            EquipWeapon(weapon);
        }

        
    }
}