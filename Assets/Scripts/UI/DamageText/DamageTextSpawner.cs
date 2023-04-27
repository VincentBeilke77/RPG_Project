using UnityEngine;

namespace RPGProject.Assets.Scripts.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            var instance = Instantiate<DamageText>(_damageTextPrefab, transform);
            instance.SetValue(damageAmount);
        }
    }
}
