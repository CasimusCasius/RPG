using UnityEngine;
namespace RPG.Core
{
    public class PeristentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawPresistentObject();
            hasSpawned = true;
        }

        private void SpawPresistentObject()
        {
            GameObject presistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(presistentObject);
        }
    }
}
