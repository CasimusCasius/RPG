using RPG.Libraries.Inventories;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if(item == null) continue;

                foreach (var modifier in item.GetAdditiveModifiers(stat))
                {
                    yield return modifier;
                }

            }
        }

        public IEnumerable<float> GetProcentageModifiers(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;

                foreach (var modifier in item.GetProcentageModifiers(stat))
                {
                    yield return modifier;
                }

            }
        }
    }
}

