using RPG.Libraries.Inventories;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Game/Inventory/Equipable Items"))]
    public class StatsEquipableItem : EquipableItem,IModifierProvider
    {
        [SerializeField] Modifier[] additiveModifiers = null;
        [SerializeField] Modifier[] percentageModifiers = null;

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            float totalModifier = 0;
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat != stat) continue;
                totalModifier += modifier.value;
                
            }
            yield return totalModifier;
        }

        public IEnumerable<float> GetProcentageModifiers(Stat stat)
        {
            float totalModifier = 0;
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat != stat) continue;
                totalModifier += modifier.value;
            }
            yield return totalModifier;
        }

    }
}
