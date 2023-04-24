using UnityEngine;
using TMPro;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthPercentage;
        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _healthPercentage.text = $"{_health.HealthPoints:0}/{_health.GetMaxHealthPoints():0}";
        }
    }
}
