using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "quicksave";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
            }
        }
    }
}