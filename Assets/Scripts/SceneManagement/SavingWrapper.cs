using RPG.Libraries.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        public const string DEFAULT_SAVE_FILE = "quicksave";

        [SerializeField] float fadeInTime = 0.5f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                StartCoroutine(LoadLastScene());
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                Delete();
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
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(DEFAULT_SAVE_FILE);
        }
    }
}