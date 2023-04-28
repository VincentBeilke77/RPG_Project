using UnityEngine;
using UnityEngine.Events;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnHitEvent;

        public void OnHit()
        {
            OnHitEvent.Invoke();
        }
    }
}
