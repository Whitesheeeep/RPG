using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 仓库的字段和属性
    private static Inventory _instance;
    public static Inventory instance { get; private set; }

    public List<ItemData> startingItems = new List<ItemData>();

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
    //角色的UI
    [SerializeField] private GameObject UI_CharacterStatsParent;
    private UI_StatSlot[] UI_CharacterStatsSlots;

    [Header("Items CoolDown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    private float FlaskCoolDown;
    private float ArmorCoolDown;

    public bool returnToInventory = true;
    #endregion


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
        UI_CharacterStatsSlots = UI_CharacterStatsParent.GetComponentsInChildren<UI_StatSlot>();


        StartingEquip();
    }

    private void StartingEquip()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    //有待改进，没必要遍历
    private void UpdateSlotUI()
    {
        for (int i = 0; i < UI_PlayerEquipmentSlots.Length; i++)
        {
            UI_PlayerEquipmentSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < UI_PlayerEquipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<Equipment_ItemData, InventoryItem> equipment in playerEquipmentDictionary)
            {
                Debug.Log("更新？");
                if (equipment.Key.equipmentType == UI_PlayerEquipmentSlots[i].EquipmentType)
                {
                    Debug.Log("更新装备UI");
                    UI_PlayerEquipmentSlots[i].UpdateSlots(equipment.Value);
                }
            } 
        }

        for (int i = 0; i < UI_EquipmentSlots.Length; i++)
        {
            UI_EquipmentSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if(i >= UI_EquipmentSlots.Length) break;
            UI_EquipmentSlots[i].UpdateSlots(inventoryItems[i]);
        }

        for (int i = 0; i < UI_StashSlots.Length; i++)
        {
            UI_StashSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            UI_StashSlots[i].UpdateSlots(stashItems[i]);
        }

        for (int i = 0; i < UI_CharacterStatsSlots.Length; i++)
        {
            UI_CharacterStatsSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Material)
            AddToMaterialStash(item);
        else if (item.itemType == ItemType.Equipment && CanAddItem())
            AddToEquipmentInventory(item);

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if(inventoryItems.Count >= UI_EquipmentSlots.Length)
        {
            Debug.Log("背包已满！");
            return false;
        }
        return true;
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

        UpdateSlotUI();
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
        Debug.Log("装备成功！" + clickedItem.name);
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
        
        if (!returnToInventory)
        {
            UpdateSlotUI();
            return;
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

    public List<InventoryItem> GetPlayerEquipmentItems() => playerEquipmentItems;

    public void SetReturnToInventory(bool value) => returnToInventory = value;

    /// <summary>
    /// 得到角色当前所装备的武器
    /// </summary>
    /// <param name="_type">输入想要查找的武器类型</param>
    /// <returns>返回对应的武器数据（没有返回null）</returns>
    public Equipment_ItemData GetEquipment(EquipmentType _type)
    {
        Equipment_ItemData equipItem = null;

        foreach (KeyValuePair<Equipment_ItemData,InventoryItem> item in playerEquipmentDictionary)
        {
            if(item.Key.equipmentType == _type)
            {
                equipItem = item.Key;
            }
        }

        return equipItem;
    }
    
    public void UseFlask()
    {
        Equipment_ItemData currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null) return; //没有药剂

        //bool canUseFlask = Time > lastTimeUsedFlask + FlaskCoolDown; 会报错，这样会导致游戏一开始的时候无法使用该能力
        bool canUseFlask = Time.time > lastTimeUsedFlask +  FlaskCoolDown;

        if (canUseFlask)
        {
            FlaskCoolDown = currentFlask.itemCooldown;
            currentFlask.ExecuteItemEffect(null);
            lastTimeUsedFlask = Time.time;
            Debug.Log("使用了药剂");
        }
        else
        {
            Debug.Log("药剂还在冷却中");
        }

    }

    public bool CanUseArmor()
    {
        Equipment_ItemData currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + ArmorCoolDown)
        {
            ArmorCoolDown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        return false;
    }
}
