using UnityEngine;

namespace RPGProject.Assets.Scripts.UI.DamageText
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject _targetToDestroy = null;

        public void DestroyTarget()
        {
            Destroy(_targetToDestroy);
        }
    }
}
