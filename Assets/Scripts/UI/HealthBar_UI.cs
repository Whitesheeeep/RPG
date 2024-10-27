using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform healthBar;
    private Slider slider;
    private CharacterStatus myStataus;


    // Start is called before the first frame update
    void Start()
    {
        entity  = GetComponentInParent<Entity>();
        healthBar = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
        myStataus = GetComponentInParent<CharacterStatus>();
        
        myStataus.OnHealthChanged += UpdateHealthBar;
        entity.OnFliped += FlipHealthBar;

        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        slider.maxValue = myStataus.GetMaxHealth();
        slider.value = myStataus.currentHealth;
    }

    private void FlipHealthBar() => healthBar.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.OnFliped -= FlipHealthBar;
        entity.status.OnHealthChanged -= UpdateHealthBar;
    }
}
