using RPG.Core;
using RPG.Movment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f;
        [SerializeField] float timeBeetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5;
        Transform target;
        float timeSinceLastAttack=0f;
        
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBeetweenAttacks)
            {
                // This will trigger Hit() event.
                GetComponent<Animator>().SetTrigger("attack");
                
                timeSinceLastAttack = 0;
            }
        }
        // AnimationEvent
        void Hit()
        {
            if (target == null) return;
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(target.position, transform.position) <= attackRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);           
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

      
    }
}