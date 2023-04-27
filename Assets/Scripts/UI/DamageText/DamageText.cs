using TMPro;
using UnityEngine;

namespace RPGProject.Assets.Scripts.UI.DamageText

{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _damageText = null;

        public void SetValue(float amount)
        {
            _damageText.text = $"{amount:0}";
        }
    }
}
