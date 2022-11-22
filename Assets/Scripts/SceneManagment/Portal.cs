using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagment
{ 
    public class Portal : MonoBehaviour
    {
        [SerializeField] int portalToSceneIndex=-1;

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
            
            print("Scene Loaded");
            Destroy(gameObject);
        }
    }
}