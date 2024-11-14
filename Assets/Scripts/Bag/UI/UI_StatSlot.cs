using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatsType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "Stat-" + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateStatValueUI()
    {
        PlayerStatus playerStats = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.StatOfType(statType).GetValue().ToString();
        }
    }
}
