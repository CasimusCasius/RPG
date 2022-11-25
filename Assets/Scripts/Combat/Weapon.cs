using RPG.Core;
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
        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetSideOfWeapon(rightHandTransform, leftHandTransform);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
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
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectile = Instantiate(projectilePrefab, GetSideOfWeapon(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target, weaponDamage);
        }

        public bool HasProjectiles() => projectilePrefab != null;
        public float GetAttackRange() => attackRange;
        public float GetWeaponDamage() => weaponDamage;

    }
}