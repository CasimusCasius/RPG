using System;
using TMPro;
using UnityEngine;

namespace RPG.Atributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerHealthValue;


        Health health;

        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            UpdatePlayer();
        }



        private void UpdatePlayer()
        {
            playerHealthValue.text = String.Format("{0:0.0}%", health.GetProcentage());
        }
    }
}
