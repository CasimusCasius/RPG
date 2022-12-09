using System;
using TMPro;
using UnityEngine;
namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelValue;
        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }
        private void Start()
        {


            levelValue.text = String.Format("{0}", baseStats.GetLevel());
        }
        private void OnEnable()
        {
            baseStats.onLevelUp += BaseStats_onLevelUp;
        }
        private void OnDisable()
        {
            baseStats.onLevelUp -= BaseStats_onLevelUp;
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
