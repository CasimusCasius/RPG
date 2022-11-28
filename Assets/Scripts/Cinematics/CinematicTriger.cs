using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinemacics
{
    public class CinematicTriger : MonoBehaviour, ISaveable
    {
        [SerializeField] bool isPlayed;

        private void OnTriggerEnter(Collider other)
        {
            if (!isPlayed && other.tag == "Player")
            {
                isPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }

        }

        public object CaptureState()
        {
            return isPlayed;
        }

        public void RestoreState(object state)
        {
            isPlayed = (bool)state;
        }
    }
}
