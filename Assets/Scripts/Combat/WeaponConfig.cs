using RPG.Atributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] float attackRange = 2f;
        [SerializeField] float weaponDamage = 5;
        [SerializeField] float procentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectilePrefab = null;

        // TODO Particle system on Atacking sword

        const string weaponName = "Weapon";


        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {

            DestroyOldWeapon(rightHandTransform, leftHandTransform);
            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                Transform handTransform = GetSideOfWeapon(rightHandTransform, leftHandTransform);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
                
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

            return weapon;
        }

       
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject damageDealer, float calculateDamage)
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
            projectile.SetProjectile(calculateDamage, attackRange, damageDealer);
        }

        public bool HasProjectiles() => projectilePrefab != null;
        public float GetAttackRange() => attackRange;
        public float GetWeaponDamage() => weaponDamage;
        public float GetWeaponProcentageBonus() => procentageBonus;

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

    }
}