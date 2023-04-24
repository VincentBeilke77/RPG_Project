using TMPro;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _experiencePoints;
        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _experiencePoints.text = $"{_experience.Points}";
        }
    }
}
