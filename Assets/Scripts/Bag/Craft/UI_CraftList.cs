using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject craftItemPrefab;
    [SerializeField] private GameObject craftItemParent;
    [SerializeField] private List<Equipment_ItemData> craftEquipment;
    //private List<UI_CraftSlot> craftSlots = new List<UI_CraftSlot>();

    private void Start()
    { 

        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftList(craftEquipment);
        SetUpDefaultWindow();
    }

    /// <summary>
    /// 将现有的CraftSlot添加到列表中
    /// </summary>
    //private void AssignCraftList() => craftSlots.AddRange(craftItemParent.GetComponentsInChildren<UI_CraftSlot>().ToList());

    public void SetUpCraftList(List<Equipment_ItemData> _craftItems)
    {
        //AssignCraftList();
        // Clear the list，以免后续重复添加
        for (int i = 0; i < craftItemParent.transform.childCount; i++)
        {
            Destroy(craftItemParent.transform.GetChild(i).gameObject);
        }
        //craftSlots.Clear();

        // Add new items
        for (int i = 0; i < _craftItems.Count; i++)
        {
            GameObject craftItem = Instantiate(craftItemPrefab, craftItemParent.transform);
            UI_CraftSlot craftSlot = craftItem.GetComponent<UI_CraftSlot>();
            craftSlot.SetUpCraftSlot(_craftItems[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList(craftEquipment);
        SetUpDefaultWindow();

    }

    public void SetUpDefaultWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI_Menu>().craftDescpWindow.SetUpCraftWindow(craftEquipment[0]);
        }
    }
}
