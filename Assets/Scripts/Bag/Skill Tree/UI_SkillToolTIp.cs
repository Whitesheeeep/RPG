using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;


    // Start is called before the first frame update
    void Start()
    {
        skillName.text = "";
        skillDescription.text = "";
        gameObject.SetActive(false);
    }

    public void ShowSkillToolTip(string _skillName, string _skillDescription)
    {
        gameObject.SetActive(true);
        skillName.text = _skillName;
        skillDescription.text = _skillDescription;
    }

    public void HideSkillToolTip()
    {
        skillName.text = "";
        skillDescription.text = "";
        gameObject.SetActive(false);
    }
}
