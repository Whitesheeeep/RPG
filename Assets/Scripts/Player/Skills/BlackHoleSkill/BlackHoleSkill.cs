using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [Space]
    [SerializeField] private float maxRadius;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCD;
    [SerializeField] private float blackHoleDuration;

    public override bool CanUseSkill()
    {
        //return base.CanUseSkill();
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        BlackHole_Controller newBlackHoleScript = newBlackHole.GetComponent<BlackHole_Controller>();
        newBlackHoleScript.SetUpBlackHole(maxRadius, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCD, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (cooldownTimer < 0)
        {
            player.CanBlackHoleReleased(true);
        }
    }
}
