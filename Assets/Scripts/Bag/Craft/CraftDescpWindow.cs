using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftDescpWindow : MonoBehaviour
{
    [SerializeField] private Image targetItemImage;
    [SerializeField] private TextMeshProUGUI targetItemName;
    [SerializeField] private TextMeshProUGUI targetItemDescp;
    [SerializeField] private Button craftButton;
    [SerializeField] private GameObject materialImagesParent;
    private Image[] materialImages => materialImagesParent.GetComponentsInChildren<Image>();

    public void SetUpCraftWindow(Equipment_ItemData _targetEquip)
    {
        craftButton.onClick.RemoveAllListeners();//去除所有监听器，防止触发其他监听器以及重复触发

        targetItemImage.sprite = _targetEquip.icon;
        targetItemName.text = _targetEquip.itemName;
        targetItemDescp.text = _targetEquip.GetDescription();

        //去除所有材料图片，如果从一个点击到另一个点击，防止材料图片重复
        for (int i = 0; i < materialImages.Length; i++)
        {
            Image image = materialImages[i];
            image.sprite = null;
            image.color = Color.clear;
            image.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        if(materialImages.Length < _targetEquip.craftRequirementMaterials.Count)
        {
            Debug.LogError("材料图片数量不足");
            return;
        }

        for (int i = 0; i < _targetEquip.craftRequirementMaterials.Count; i++)
        {
           
            materialImages[i].sprite = _targetEquip.craftRequirementMaterials[i].itemData.icon;
            materialImages[i].color = Color.white;
            materialImages[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _targetEquip.craftRequirementMaterials[i].stackSize.ToString();
        }

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_targetEquip, _targetEquip.craftRequirementMaterials));
    }
}
