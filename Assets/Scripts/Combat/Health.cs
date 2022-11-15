using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{


    public class Health : MonoBehaviour
    {
        [SerializeField]float health = 100f;
        bool isDead;
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health-damage,0);
            if(!isDead && health <= 0) 
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
            }

        }

    }
}