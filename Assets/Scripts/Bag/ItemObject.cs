using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    //只会在 Editor状态中启用，在 Play 状态下不会启用。OnValidate 会在每次在 inspector 中修改了这个脚本的值后被调用
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object-"+ itemData.itemName; 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
