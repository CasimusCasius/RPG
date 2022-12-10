using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy;
    void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            if (targetToDestroy == null) { targetToDestroy = gameObject; }
            Destroy(targetToDestroy);
        }
    }
}
