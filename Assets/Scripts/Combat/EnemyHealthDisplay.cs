using RPG.Atributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI enemyHealthValue;
        GameObject player;
        

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            Health target = player.GetComponent<Fighter>().GetTarget();
            if (target != null)
            {
                enemyHealthValue.text = String.Format("{0:0.0}%", target.GetProcentage());
            }
            else
            {
                enemyHealthValue.text = "N/A";
            }
        }
    }
}
