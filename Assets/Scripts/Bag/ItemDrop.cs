using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> possibleDropList = new List<ItemData>();

    [SerializeField] private GameObject dropItemPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0f, 100f) < possibleDrop[i].dropChance)
            {
                possibleDropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i < amountOfDrop; i++)
        {
            ItemData dropItem = possibleDropList[Random.Range(0, possibleDropList.Count-1)];
            DropItem(dropItem);
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        Debug.Log("开始初始化掉落物品");
        GameObject dropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        dropItem.GetComponent<ItemObject>().SetUpItem(_itemData, new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 7f)));
    }
}
