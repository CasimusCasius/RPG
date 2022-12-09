using UnityEngine;
namespace RPG.Core
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject[] objectsToDestroy;


        public void DestroyObjects()
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
        }
    }
}
