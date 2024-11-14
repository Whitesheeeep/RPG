using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effects/Heal Effect")]
public class HealEffect : ItemEffects
{

    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform targetTransform)
    {
        PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();

        float healAmount = playerStatus.GetMaxHealth() * healPercent;

        playerStatus.IncreaseHealthyBy(healAmount);
    }
}
