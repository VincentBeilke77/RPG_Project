using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float _health = 100f;

        private bool _isDead = false;

        public bool IsDead { get { return _isDead; } set { _isDead = value; } }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);

            if (_health == 0)
            {
                Die(); 
            }
        }

        private void Die()
        {
            if (_isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            _isDead = true;
        }
    }
}
