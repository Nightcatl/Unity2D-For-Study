using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private CharacterStats stat;
    [SerializeField] private Slider healthBar;

    private void Start()
    {
        stat = GetComponentInParent<CharacterStats>();

        if (stat != null)
            stat.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.maxValue = stat.MaxHealth.GetValue();
        healthBar.value = stat.currentHealth;
    }
}
