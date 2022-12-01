using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour,IRaycastable

    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 5f;


        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);

        }

        private void ShowPickup(bool shouldShow)
        {
            Collider collider = GetComponent<Collider>();
            collider.enabled = shouldShow;
            foreach (Transform child in transform)  
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
        public bool HandleRaycast(PlayerController callingControler)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingControler.GetComponent<Fighter>());

            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
