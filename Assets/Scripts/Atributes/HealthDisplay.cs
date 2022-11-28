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
        private void Start()
        {

            UpdatePlayerHealth();
        }
        private void OnEnable()
        {
            health.onHealthChanged += Health_OnHealthChanged;
        }
        private void OnDisable()
        {
            health.onHealthChanged -= Health_OnHealthChanged;
        }

        private void Health_OnHealthChanged()
        {
            UpdatePlayerHealth();
        }



        private void UpdatePlayerHealth()
        {
            playerHealthValue.text = String.Format("{0:0} / {1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
