using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region �����������
    private UI_Menu uI_Menu;
    #endregion

    [SerializeField] private int skillPrice;
    [SerializeField] private SkillTreeLock[] shouldBeLocked;
    [SerializeField] private SkillTreeLock[] shouldBeUnLocked;

    #region ��������
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    #endregion ��������

    [SerializeField] private Color LockedSkillColor;

    public bool isLocked = true;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => CanUnlockSkill());
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isLocked) GetComponent<Image>().color = LockedSkillColor;
        

        uI_Menu = FindObjectOfType<UI_Menu>();
    }

    private void OnValidate()
    {
        gameObject.name = "Skill-" + skillName;
    }

    //�鿴�Ƿ���Խ������ܣ������򽫼�������ͼ���Ϊ����״̬
    private void CanUnlockSkill()
    {
        foreach (var skill in shouldBeLocked)
        {
            if (!skill.isLocked)
            {
                return;
            }
        }

        foreach (var skill in shouldBeUnLocked)
        {
            if(skill.isLocked)
            {
                return;
            }

        }

        if (!PlayerManager.instance.HaveEnoughMoney(skillPrice)) return;

        isLocked = false;
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uI_Menu.skillToolTip.ShowSkillToolTip(skillName, skillDescription);

        float xOffset = Input.mousePosition.x < Screen.width / 2? 100: -100;
        float yOffset = Input.mousePosition.y < Screen.height / 2 ? 100 : -100;

        uI_Menu.skillToolTip.transform.position = new Vector2(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData) => uI_Menu.skillToolTip.HideSkillToolTip();
}
