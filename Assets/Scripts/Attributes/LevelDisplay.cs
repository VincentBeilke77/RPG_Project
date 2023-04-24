using RPGProject.Assets.Scripts.Stats;
using TMPro;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _level;
        private BaseStats _baseStats;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _level.text = $"{_baseStats.GetLevel()}";
        }
    }
}
