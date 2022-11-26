using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] float projectileRange = 20;
        Health target;
        Fighter damageDealer;
        float damage = 0;
        float timeOfLife;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            timeOfLife = (1/projectileSpeed) * projectileRange ; //
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

        public void SetTarget(Health target, float damage, Fighter damageDealer)
        {
            this.target = target;
            this.damage = damage;
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
            Debug.Log("Hit "+ other.gameObject.name);

            if (other.gameObject == damageDealer.gameObject) { 
                
                Debug.Log ("Thats me" + damageDealer.gameObject.name);
                return; }

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
