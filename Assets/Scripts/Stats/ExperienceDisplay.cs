
using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI experienceValue;
        Experience playerExpPoints;
        void Start()
        {
            playerExpPoints =GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            experienceValue.text = String.Format( "{0:0}", playerExpPoints.GetExperience());
        }



        // Update is called once per frame
        void Update()
        {
            experienceValue.text = String.Format("{0:0}", playerExpPoints.GetExperience());
        }
    }
}
