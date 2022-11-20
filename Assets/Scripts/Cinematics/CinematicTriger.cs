using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinemacics
{
    public class CinematicTriger : MonoBehaviour
    {
        bool isPlayed =  false;
        private void OnTriggerEnter(Collider other)
        {
            if(!isPlayed && other.tag == "Player")
            {
                isPlayed = true;
                GetComponent<PlayableDirector>().Play();
               
            }   
           
        }
    }
}
