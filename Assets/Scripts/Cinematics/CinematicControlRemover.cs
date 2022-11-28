using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinemacics
{ 
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }
        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }


        void DisableControl(PlayableDirector playableDirector)
        {
            if(player == null) { return; }
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            if (player == null) { return; }
            
            player.GetComponent<PlayerController>().enabled = true;
        }
        

    }
}