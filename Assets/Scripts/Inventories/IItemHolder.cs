using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Inventories
{
    public interface IItemHolder
    {
        public InventoryItem GetItem();
    }
}
