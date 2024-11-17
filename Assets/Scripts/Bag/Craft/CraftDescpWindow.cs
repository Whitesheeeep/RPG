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
        craftButton.onClick.RemoveAllListeners();//ȥ�����м���������ֹ���������������Լ��ظ�����

        targetItemImage.sprite = _targetEquip.icon;
        targetItemName.text = _targetEquip.itemName;
        targetItemDescp.text = _targetEquip.GetDescription();

        //ȥ�����в���ͼƬ�������һ���������һ���������ֹ����ͼƬ�ظ�
        for (int i = 0; i < materialImages.Length; i++)
        {
            Image image = materialImages[i];
            image.sprite = null;
            image.color = Color.clear;
            image.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        if(materialImages.Length < _targetEquip.craftRequirementMaterials.Count)
        {
            Debug.LogError("����ͼƬ��������");
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
