using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        public const string DEFAULT_SAVE_FILE = "quicksave";

        [SerializeField] float fadeInTime = 0.5f;

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            fader.FadeIn(fadeInTime);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                Load();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
        }
    }
}