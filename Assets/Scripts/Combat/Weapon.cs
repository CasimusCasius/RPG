using RPG.Atributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] float attackRange = 2f;
        [SerializeField] float weaponDamage = 5;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectilePrefab = null;

        // TODO Particle system on Atacking sword

        const string weaponName = "Weapon";


        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {

            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equippedPrefab != null)
            {
                Transform handTransform = GetSideOfWeapon(rightHandTransform, leftHandTransform);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }
            var overrideControler = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideControler != null)
            {
                animator.runtimeAnimatorController = overrideControler.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            }
            if (oldWeapon == null) { return; }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetSideOfWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
            }

            return handTransform;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject damageDealer)
        {
            // TODO arrow parabolic move
            // Z = distance
            // v0 = sqrt(Z * g / sin(2*alpha0)
            // alphax= arctg(tg(alpha0) - (g*dx/v0*v0*cos(alpha0)*cos(alpha0))) // rotation 
            // x(t) = v0*t *sin(alpha)
            // y(t) = v0 *t * sin(alpha) - g*t*t/2
            // alpha0 = arcsin(Z * g/(v0*v0))/2


            Projectile projectile = Instantiate(projectilePrefab, GetSideOfWeapon(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target);
            projectile.SetProjectile(weaponDamage, attackRange, damageDealer);
        }

        public bool HasProjectiles() => projectilePrefab != null;
        public float GetAttackRange() => attackRange;
        public float GetWeaponDamage() => weaponDamage;

    }
}