using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private bool canAttack;

    [SerializeField] private GameObject clonePrefab;

    [Header("PlayerCloneInfo")]
    [SerializeField] public float cloneExistDuration;
    [SerializeField] private float cloneFadingSpeed;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    public void CreateClone(Transform newTransform, float offset = 0f)
    {
         GameObject newClone = Instantiate(clonePrefab);
        Transform closestEnemy = FindClosestEnemy(newTransform);
        newClone.GetComponent<PlayerCloneController>().SetUpPlayerClone(newTransform, cloneExistDuration,
            cloneFadingSpeed,canAttack, closestEnemy, 
            canDuplicateClone, chanceToDuplicate,
            offset);
    }
}
