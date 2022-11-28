using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetProcentageModifiers(Stat stat);
    }
}
