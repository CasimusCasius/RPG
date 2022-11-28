
using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI experienceValue;
        Experience playerExpPoints;
        private void Awake()
        {
            playerExpPoints = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }
        void Start()
        {
            experienceValue.text = String.Format( "{0:0}", playerExpPoints.GetExperience());
        }
        void Update()
        {
            experienceValue.text = String.Format("{0:0}", playerExpPoints.GetExperience());
        }
    }
}
