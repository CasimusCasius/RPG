using RPG.Movment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour
    {
        [SerializeField] float attackRange = 2f;
        Transform target;
        
        private void Update()
        {
            if (target == null) return;
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
            }


        }

        private bool IsInRange()
        {
            return Vector3.Distance(target.position, transform.position) <= attackRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}