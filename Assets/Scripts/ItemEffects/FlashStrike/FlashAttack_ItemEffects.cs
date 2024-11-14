using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashAttack_ItemEffects", menuName = "Data/Item Effects/FlashAttack_ItemEffects")]
public class FlashAttack_ItemEffects : ItemEffects
{
    [SerializeField] private GameObject flashPrefab;

    private GameObject newFlash;
    public override void ExecuteEffect(Transform targetTransform)
    {
        newFlash  = Instantiate(flashPrefab, targetTransform.position, Quaternion.identity);
        
    }

    
}
