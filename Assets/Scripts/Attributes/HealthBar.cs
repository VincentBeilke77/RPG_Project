using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _healthComponent = null;
        [SerializeField] private RectTransform _foreground = null;
        [SerializeField] private Canvas _rootCanvas = null;

        private void OnEnable()
        {
            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDeath -= OnDeath;
        }

        private void OnHealthChanged()
        {
            _foreground.localScale = new Vector3(_healthComponent.GetFraction(), 1, 1);
            _rootCanvas.enabled = true;
        }

        private void OnDeath()
        {
            _rootCanvas.enabled = false;
        }
    }
}
