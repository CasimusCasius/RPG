using RPG.Atributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform foregroundImage;
    [SerializeField] float timeToHideHealthBar=3f;
    [SerializeField] Canvas rootCanvas;
    Health health;
    CombatTarget combatTarget;

    float timeBeetwenAttacks = float.MaxValue;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        combatTarget = GetComponentInParent<CombatTarget>();
    }
    private void OnEnable()
    {
        health.onHealthChanged += Health_onHealthChanged;
        health.onDead += Die;
        combatTarget.OnAttacked += CombatTarget_onAttacked;
    }
    private void Update()
    {
        timeBeetwenAttacks+= Time.deltaTime;
        if (timeBeetwenAttacks > timeToHideHealthBar) HideHealBar();
    }

    private void OnDisable()
    {
        health.onHealthChanged-= Health_onHealthChanged;
        health.onDead-= Die;
        combatTarget.OnAttacked -= CombatTarget_onAttacked;
    }
    private void CombatTarget_onAttacked()
    {
        timeBeetwenAttacks = 0f;
        ShowHealthBar();
    }
    private void Health_onHealthChanged()
    {
        if (health == null) return;
        ShowHealthBar();
        foregroundImage.GetComponent<Image>().fillAmount = health.GetProcentage() / 100;
        timeBeetwenAttacks = 0;
    }
    private void Die()
    {
        foregroundImage.GetComponent<Image>().fillAmount = 0;
        StartCoroutine(DisableHealthBar());
    }
    private void ShowHealthBar()
    {
        
        rootCanvas.enabled = true;
    }
    private void HideHealBar()
    {
        rootCanvas.enabled = false;
    }

    private IEnumerator DisableHealthBar()
    {
        yield return new WaitForSeconds(timeToHideHealthBar);
        HideHealBar();
    }
}
