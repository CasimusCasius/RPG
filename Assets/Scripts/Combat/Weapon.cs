using RPG.Core;
using System;
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
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
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
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, Fighter damageDealer)
        {
            
            Projectile projectile = Instantiate(projectilePrefab, GetSideOfWeapon(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target);
            projectile.SetProjectile(weaponDamage, attackRange, damageDealer);
        }

        public bool HasProjectiles() => projectilePrefab != null;
        public float GetAttackRange() => attackRange;
        public float GetWeaponDamage() => weaponDamage;

    } 
}