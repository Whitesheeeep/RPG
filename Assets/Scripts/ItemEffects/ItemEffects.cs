using UnityEngine;


public class ItemEffects : ScriptableObject
{
    public virtual void ExecuteEffect(Transform targetTransform)
    {
        Debug.Log("执行物品效果");
    }

    
}