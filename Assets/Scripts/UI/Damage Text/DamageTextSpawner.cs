using System;
using UnityEngine;
namespace RPG.UI.DamageText
{

    public class DamageTextSpawner : MonoBehaviour
    {

        [SerializeField] DamageText damageTextPrefab;

        public void Spawn(float damage)
        {
            DamageText spawnedText = Instantiate<DamageText>(damageTextPrefab, transform);
            spawnedText.SetDamageText(String.Format("{0:0}", damage));
        }
    }
}