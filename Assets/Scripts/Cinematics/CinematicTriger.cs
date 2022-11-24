using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinemacics
{
    public class CinematicTriger : MonoBehaviour, ISaveable
    {
        [SerializeField] bool isPlayed = false;

        public object CaptureState()
        {
            return isPlayed;
        }

        public void RestoreState(object state)
        {
            isPlayed = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isPlayed && other.tag == "Player")
            {
                isPlayed = true;
                GetComponent<PlayableDirector>().Play();

            }

        }
    }
}
