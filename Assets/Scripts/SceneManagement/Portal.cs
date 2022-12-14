using RPG.Control;
using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F, G
        }

        [SerializeField] int portalToSceneIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;



        private void Awake()
        {
            FindObjectOfType<SavingWrapper>();

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            if (portalToSceneIndex < 0)
            {
                Debug.LogError("Scene to load not set!");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            Fader fader = FindObjectOfType<Fader>();

            DisablePlayerControl();
            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(portalToSceneIndex);
            DisablePlayerControl();
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            EnablePlayerControl();
            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);

            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
        private Portal GetOtherPortal()
        {

            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                {
                    return portal;
                }
            }
            return null;
        }

        private void DisablePlayerControl()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player == null) { return; }
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnablePlayerControl()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player == null) { return; }

            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}