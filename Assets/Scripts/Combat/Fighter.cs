using RPG.Atributes;
using RPG.Core;
using RPG.Movment;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction, ISaveable,IModifierProvider
    {

        [SerializeField] float timeBeetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
       

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
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
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();

            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }
        public IEnumerable<float> GetProcentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponProcentageBonus();
            }
        }
        public Health GetTarget() => target;

        // AnimationEvent
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectiles())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject,damage);
            }
            else
            {
                
                target.TakeDamage(gameObject, damage);
            }
        }
        // AnimationEvent
        void Shoot() { Hit(); }

        private bool IsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.GetAttackRange();
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
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }

        
    }
}