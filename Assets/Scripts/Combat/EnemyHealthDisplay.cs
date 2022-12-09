using RPG.Atributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI enemyHealthValue;
        Fighter fighter;


        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();

        }

        private void Update()
        {
            UpdateTargetHeal();
        }

        private void UpdateTargetHeal()
        {

            if (fighter.GetTarget() == null)
            {
                enemyHealthValue.text = "N/A";
                return;
            }

            Health health = fighter.GetTarget().GetComponent<Health>();
            enemyHealthValue.text = String.Format("{0:0} / {1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
