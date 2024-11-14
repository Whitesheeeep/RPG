using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    public void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }

    private void OnValidate()
    {
        if (itemData == null)
        {
            return;
        }
        
        gameObject.name = "Item Object-" + itemData.itemName;

    }

    private void VisualImage()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object-" + itemData.itemName;
    }

    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        VisualImage();
    }

    public void PickUpItem()
    {
        if(!Inventory.instance.CanAddItem())
        {
            rb.velocity = new Vector2(0, 5);
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}