using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelValue;
        BaseStats baseStats;

        private void Start()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            levelValue.text = String.Format("{0}", baseStats.GetLevel());
        }
        private void Update()
        {
            levelValue.text = String.Format("{0}", baseStats.GetLevel());
        }

    }
}
