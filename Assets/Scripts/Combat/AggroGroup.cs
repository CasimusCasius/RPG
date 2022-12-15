using RPG.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;


        private void Start ()
        {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (var fighter in fighters)
            {
                
                fighter.GetComponent<AIConversant>().enabled = !shouldActivate;

                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if (target != null) target.enabled = shouldActivate;
                fighter.enabled = shouldActivate;
            }

        }

    }
}
