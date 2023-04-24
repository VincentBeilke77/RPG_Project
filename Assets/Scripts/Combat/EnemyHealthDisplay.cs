using UnityEngine;
using TMPro;
using RPGProject.Assets.Scripts.Attributes;

namespace RPGProject.Assets.Scripts.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthPercentage;
        private Fighter _fighter = null;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.Target == null)
            {
                _healthPercentage.text = "N/A";
                return;
            }
            
            _healthPercentage.text = $"{_fighter.Target.GetPercentage():0}%";
        }
    }
}
