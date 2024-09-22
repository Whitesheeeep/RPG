using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTirggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
