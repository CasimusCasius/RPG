using RPG.Atributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 20;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] UnityEvent onHit;
        [SerializeField] AudioSource hitSFX = null;
        [SerializeField] AudioSource launchSFX= null;
            


        float projectileRange;
        Health target;
        GameObject damageDealer;
        float damage = 0;
        float timeOfLife;

        private void Start()
        {

            transform.LookAt(GetAimLocation());
            float timeOfLifeFactor = 1.5f;
            timeOfLife = (1 / projectileSpeed) * projectileRange * timeOfLifeFactor;
        }
        private void Update()
        {
            if (target == null) { return; }
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target)
        {
            this.target = target;
        }

        public void SetProjectile(float damage, float weaponRange, GameObject damageDealer)
        {
            this.damage = damage;
            this.projectileRange = weaponRange;
            this.damageDealer = damageDealer;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) { return target.transform.position; }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == damageDealer.gameObject) { return; }

            if (other.TryGetComponent<Health>(out Health enemy))
            {
                if (enemy.IsDead()) { return; }
                StartProjectalHitEffect();
                projectileSpeed = 0;
                enemy.TakeDamage(damageDealer, damage);

                Destroy(gameObject, 0.2f);
            }

            Destroy(gameObject, timeOfLife);
        }

        private void StartProjectalHitEffect()

        {
            onHit?.Invoke();
            if (hitEffect == null) { return; }

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, out hit)) return;
            Instantiate(hitEffect, hit.point, Quaternion.identity);

        }
    }
}
