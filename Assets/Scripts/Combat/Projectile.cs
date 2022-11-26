using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 20;
        [SerializeField] bool isHoming = false;
        float projectileRange;
        Health target;
        Fighter damageDealer;
        float damage = 0;
        float timeOfLife;
        
        private void Start()
        {

            transform.LookAt(GetAimLocation());
            float timeOfLifeFactor = 1.5f;
            timeOfLife = (1 / projectileSpeed) * projectileRange*timeOfLifeFactor  ;
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

        public void SetProjectile (float damage,float weaponRange ,Fighter damageDealer)
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
                enemy.TakeDamage(damage);
                if (enemy.IsDead()) { return; }
                Destroy(gameObject);
            }

            Destroy(gameObject,timeOfLife);
        }
    }
}
