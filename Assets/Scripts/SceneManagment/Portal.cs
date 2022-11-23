
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        [SerializeField] int portalToSceneIndex=-1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        

        private void Start()
        {
           
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag =="Player") 
            {
                StartCoroutine(Transition());
                
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(portalToSceneIndex);
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            bool succes = player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            Debug.Log(otherPortal.spawnPoint.position);
            player.transform.rotation= otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if(portal.destination != destination) continue;
                {
                    return portal;
                }
            }
            return null;
        }
    }
}