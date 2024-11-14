using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireAndIce_ItemEffects", menuName = "Data/Item Effects/FireAndIce_ItemEffects")]
public class FireAndIce_ItemEffects : ItemEffects
{
    [SerializeField] private GameObject FireAndIcePrefab;
    public override void ExecuteEffect(Transform targetTransform)
    {
        Player player = PlayerManager.instance.player;

        //第三次攻击才触发冰火攻击
        bool thirdAttack = player.PrimaryAttack.attackCounter == 2;
        Debug.Log(player.PrimaryAttack.attackCounter);
        Debug.Log(thirdAttack);

        if (thirdAttack)
        {
            GameObject fireAndIce = Instantiate(FireAndIcePrefab, player.transform.position, player.transform.rotation);
            Destroy(fireAndIce, 5f);
        }
    }
}
