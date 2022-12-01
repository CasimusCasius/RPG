using RPG.Atributes;
using RPG.Control;
using RPG.Movment;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Atributes.Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        
        public event Action OnAttacked;

        public bool HandleRaycast(PlayerController callingControler)
        {
            if (!callingControler.GetComponent<Fighter>().CanAttack(this.gameObject)) return false;

            if (Input.GetMouseButtonDown(0))
            {
                OnAttacked?.Invoke();
                Attack(callingControler.GetComponent<Fighter>());
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        private void Attack(Fighter fighter)
        {
            if (!fighter.CanAttack(gameObject)) return;

            if (Input.GetMouseButton(0))
            {
                fighter.Attack(gameObject);
            }   
        }
    }
}
