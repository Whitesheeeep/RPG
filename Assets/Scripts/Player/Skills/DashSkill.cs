using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [SerializeField] private SkillTreeLock dashUnlockButton;
    public  bool dashUnlockEnabled;

    [SerializeField] private SkillTreeLock cloneOnDashUnlocked;
    public bool cloneOnDashUnlockedEnabled;

    [SerializeField] private SkillTreeLock cloneOnArrivalUnlocked;
    public bool cloneOnArrivalUnlockedEnabled;


    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(dashUnlock);
        cloneOnDashUnlocked.GetComponent<Button>().onClick.AddListener(cloneOnDashUnlock);
        cloneOnArrivalUnlocked.GetComponent<Button>().onClick.AddListener(cloneOnArrivalUnlock);
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void dashUnlock()
    {
        if(!dashUnlockButton.isLocked) dashUnlockEnabled = true;

    }

    private void cloneOnDashUnlock()
    {
        if (!cloneOnDashUnlocked.isLocked) cloneOnDashUnlockedEnabled = true;
    }

    private void cloneOnArrivalUnlock()
    {
        if (!cloneOnArrivalUnlocked.isLocked) cloneOnArrivalUnlockedEnabled = true;
    }


    public void CreateOnStart()
    {
        if (cloneOnDashUnlockedEnabled)
        {
            SkillManager.instance.clone.CreateClone(player.transform);
        }
    }

    public void CreateOnEnd()
    {
        if (cloneOnArrivalUnlockedEnabled)
        {
            SkillManager.instance.clone.CreateClone(player.transform);
        }
    }
}
