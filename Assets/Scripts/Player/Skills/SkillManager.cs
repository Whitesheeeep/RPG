using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public SwordSkill swordSkill { get; private set; }
    public BlackHoleSkill blackHoleSkill { get; private set; }
    public CrystalSkill crystalSkill { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            Debug.Log("There are more than one SkillManager in the scene");
        }
        else
        {
            instance = this;
        }
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        blackHoleSkill = GetComponent<BlackHoleSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }

}
