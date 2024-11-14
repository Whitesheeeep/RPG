using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region 其他对象组件
    private UI_Menu uI_Menu;
    #endregion

    [SerializeField] private SkillTreeLock[] shouldBeLocked;
    [SerializeField] private SkillTreeLock[] shouldBeUnLocked;

    #region 描述技能
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    #endregion 描述技能

    [SerializeField] private Color LockedSkillColor;

    public bool isLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = LockedSkillColor;
        GetComponent<Button>().onClick.AddListener(() => CanUnlockSkill());

        uI_Menu = FindObjectOfType<UI_Menu>();
    }

    private void OnValidate()
    {
        gameObject.name = "Skill-" + skillName;
    }

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

        isLocked = false;
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
