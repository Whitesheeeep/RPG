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

    public void CreateClone(Transform newTransform)
    {
        GameObject gameObject = Instantiate(clonePrefab);
        gameObject.GetComponent<PlayerCloneController>().SetUpPlayerClone(newTransform, cloneExistDuration,cloneFadingSpeed,canAttack);
    }
}
