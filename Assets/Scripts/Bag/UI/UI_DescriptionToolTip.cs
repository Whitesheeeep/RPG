using UnityEngine;
using TMPro;

public class UI_DescriptionToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descrpHeader;
    [SerializeField] private TextMeshProUGUI descrpType;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private float defaultHeaderFontSize = 32;

    private void OnValidate()
    {
        defaultHeaderFontSize = descrpHeader.fontSize;
    }

    public void ShowToolTip(Equipment_ItemData EquipmentItem)
    {
        if (EquipmentItem != null)
        {
            descrpHeader.text = EquipmentItem.itemName;
            descrpType.text = EquipmentItem.equipmentType.ToString();
            description.text = EquipmentItem.GetDescription();//怎么获取描述信息可以重点描述

            if (descrpHeader.text.Length > 12)
            {
                descrpHeader.fontSize = descrpHeader.fontSize * 0.7f;
            }
            else
            {
                descrpHeader.fontSize = defaultHeaderFontSize;
            }

            gameObject.SetActive(true);
        }
    }

    public void HideToolTip()
    {
        descrpHeader.fontSize = defaultHeaderFontSize;
        gameObject.SetActive(false);
    }


}
