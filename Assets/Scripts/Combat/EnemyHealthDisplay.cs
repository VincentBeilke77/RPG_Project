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
            
            Health _health = _fighter.Target;
            _healthPercentage.text = $"{_health.HealthPoints:0}/{_health.GetMaxHealthPoints():0}";
        }
    }
}
