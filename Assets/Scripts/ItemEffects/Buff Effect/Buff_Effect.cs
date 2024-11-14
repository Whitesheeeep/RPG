using UnityEngine;



[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effects/Buff Effect")]
public class Buff_Effect : ItemEffects
{
    private PlayerStatus playerStatus;

    [SerializeField] private float buffDuration;
    [SerializeField] private StatsType buffStatsType;
    [Tooltip("buff 增加的数值")]
    [SerializeField] private float buffValue;

    public override void ExecuteEffect(Transform targetTransform = null)
    {
        playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        playerStatus.StatsIncreaseBy(buffValue, buffDuration, playerStatus.StatOfType(buffStatsType));
    }

    
}

