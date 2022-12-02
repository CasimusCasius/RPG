using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageValue;

        string damageText;
        private void Start()
        {
            damageValue.text = damageText;
        }


        public void SetDamageText(string damageText)
        {
            this.damageText = damageText;
        }

        public void DestroyText()
        {
            Destroy(this.gameObject);
        }
    }
}