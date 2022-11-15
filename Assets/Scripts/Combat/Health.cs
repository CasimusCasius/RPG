using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{


    public class Health : MonoBehaviour
    {
        [SerializeField]float healthPoints = 100f;
        bool isDead;
        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints-damage,0);
            if(!isDead && healthPoints == 0) 
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
            }

        }

        public bool IsDead()
        {
            return isDead;
        }

    }
}