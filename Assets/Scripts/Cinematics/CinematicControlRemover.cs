using RPGProject.Assets.Scripts.Controllers;
using RPGProject.Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPGProject.Assets.Scripts.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        private GameObject _player;

        private void Start()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _player = GameObject.FindWithTag("Player");

            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            print("DisableControl");
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("EnableControl");
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
