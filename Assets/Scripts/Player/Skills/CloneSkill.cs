using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private bool canAttack;

    [SerializeField] private GameObject clonePrefab;

    //能否在冲击开始时生成影子的技能锁
    [SerializeField] private bool canCreateCloneOnStart;
    //能否在冲击结束时生成影子的技能锁
    [SerializeField] private bool canCreateCloneOnEnd;

    [Header("PlayerCloneInfo")]
    [SerializeField] public float cloneExistDuration;
    [SerializeField] private float cloneFadingSpeed;

    public void CreateClone(Transform newTransform, float offset = 0f)
    {
         GameObject newClone = Instantiate(clonePrefab);
        Transform closestEnemy = FindClosestEnemy(newTransform);
        newClone.GetComponent<PlayerCloneController>().SetUpPlayerClone(newTransform, cloneExistDuration,cloneFadingSpeed,canAttack, closestEnemy, offset);
    }

    public void CreateOnStart()
    {
        if (canCreateCloneOnStart)
        {
            CreateClone(player.transform);
        }
    }

    public void CreateOnEnd()
    {
        if (canCreateCloneOnEnd)
        {
            CreateClone(player.transform);
        }
    }
}
