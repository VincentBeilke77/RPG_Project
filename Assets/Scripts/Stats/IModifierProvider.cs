using RPGProject.Assets.Scripts.Stats;
using System.Collections.Generic;

namespace RPGProject
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}
