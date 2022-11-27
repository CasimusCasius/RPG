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
            baseStats.onLevelUp += BaseStats_onLevelUp;
            levelValue.text = String.Format("{0}", baseStats.GetLevel());
        }

        private void BaseStats_onLevelUp()
        {
            UpdateLevel();
        }

        private void UpdateLevel()
        {
            levelValue.text = String.Format("{0}", baseStats.GetLevel());
        }

    }
}
