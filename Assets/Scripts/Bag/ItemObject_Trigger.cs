using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<PlayerStatus>() != null)
            {
                if (!collision.GetComponentInParent<PlayerStatus>().isDead)
                {
                    itemObject.PickUpItem();
                }
            }
        }
    }
}
