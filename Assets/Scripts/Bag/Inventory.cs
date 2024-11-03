using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    public static Inventory instance { get; private set; }


    //Dictionary 在 Unity 的 inspector 中不可见，所以我们需要一个 List 来存储所有的 InventoryItem
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> playerEquipmentItems;
    public Dictionary<Equipment_ItemData, InventoryItem> playerEquipmentDictionary;

    [Header("Inventory UI")]
    //装备到角色的 UI
    [SerializeField] private GameObject UI_PlayerEquipmentParent;
    private UI_PlayerEquipment[] UI_PlayerEquipmentSlots;
    //装备栏的 UI
    [SerializeField] private GameObject UI_InventoryForEquipmentParent;
    private UI_ItemSlot[] UI_EquipmentSlots;
    //材料栏的 UI
    [SerializeField] private GameObject UI_StashForMaterialParent;
    private UI_ItemSlot[] UI_StashSlots;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        playerEquipmentItems = new List<InventoryItem>();
        playerEquipmentDictionary = new Dictionary<Equipment_ItemData, InventoryItem>();

        UI_EquipmentSlots = UI_InventoryForEquipmentParent.GetComponentsInChildren<UI_ItemSlot>();
        UI_StashSlots = UI_StashForMaterialParent.GetComponentsInChildren<UI_ItemSlot>();
        UI_PlayerEquipmentSlots = UI_PlayerEquipmentParent.GetComponentsInChildren<UI_PlayerEquipment>();
    }

    //有待改进，没必要遍历
    private void UpdateSlotUI()
    {
        for (int i = 0; i < UI_PlayerEquipmentSlots.Length; i++)
        {
            UI_PlayerEquipmentSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < playerEquipmentItems.Count; i++)
        {
            UI_PlayerEquipmentSlots[i].UpdateSlots(playerEquipmentItems[i]);
        }

        for (int i = 0; i < UI_EquipmentSlots.Length; i++)
        {
            UI_EquipmentSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            UI_EquipmentSlots[i].UpdateSlots(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            UI_StashSlots[i].UpdateSlots(stashItems[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Material)
            AddToMaterialStash(item);


        if (item.itemType == ItemType.Equipment)
            AddToEquipmentInventory(item);

        UpdateSlotUI();
    }

    private void AddToMaterialStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stashDictionary.Add(item, newItem);
            stashItems.Add(newItem);
        }
    }

    private void AddToEquipmentInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryDictionary.Add(item, newItem);
            inventoryItems.Add(newItem);
        }
        UpdateSlotUI();
    }

    public void RemoveItemFromStash(ItemData itemData)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stashItems.Remove(value);
                stashDictionary.Remove(itemData);
            }
            else
            {
                value.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public void RemoveItemFromInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
                //if(value.stackSize <= 0)
                //{
                //    inventoryItems.Remove(value);
                //    inventoryDictionary.Remove(OldEquipment);
                //}
            }
        }

        UpdateSlotUI();
    }

    public void PlayerEquipWith(Equipment_ItemData clickedItem)
    {
        if(playerEquipmentDictionary.TryGetValue(clickedItem, out InventoryItem value))
        {
            Debug.Log("你已经装备该物品了！" + value.itemData.name);
            return;
        }
        else
        {   
            foreach (InventoryItem item in playerEquipmentItems)
            {
                if ((item.itemData as Equipment_ItemData).equipmentType == clickedItem.equipmentType)
                {
                    SwitchEquipment(clickedItem, item.itemData as Equipment_ItemData);
                    return;
                }
            }

            InventoryItem newItem = new InventoryItem(clickedItem);
            playerEquipmentDictionary.Add(clickedItem, newItem);
            playerEquipmentItems.Add(newItem);
            clickedItem.AddModifiers();
        }

        RemoveItemFromInventory(clickedItem);
    }

    public void PlayerUnEquipWith(Equipment_ItemData clickedItem)
    {
        if (playerEquipmentDictionary.TryGetValue(clickedItem, out InventoryItem value))
        {
            playerEquipmentItems.Remove(value);
            playerEquipmentDictionary.Remove(clickedItem);
            clickedItem.RemoveModifiers();
        }
        

        AddToEquipmentInventory(clickedItem);
    }
    private void SwitchEquipment(Equipment_ItemData newEquipment, Equipment_ItemData OldEquipment)
    {
        PlayerUnEquipWith(OldEquipment);

        InventoryItem newInventoryItem = new InventoryItem(newEquipment);
        playerEquipmentDictionary.Add(newEquipment, newInventoryItem);
        playerEquipmentItems.Add(newInventoryItem);
        newEquipment.AddModifiers();

        RemoveItemFromInventory(newEquipment);
    }



}
