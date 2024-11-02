using System;
using UnityEngine;

enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    [SerializeField] private SwordType swordType;

    [Header("SKill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private /*Vector2*/float launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTime = 0.1f;
    [SerializeField] private float returnSpeed = 30f;
    private Vector2 finalDir;


    [Header("Dots Info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int dotsCount;
    [SerializeField] private float dotSpacing;
    [SerializeField] private Transform dotsParent;
    private static GameObject[] dots;

    [Header("Bounce Info")]
    [SerializeField] private float boucingSpeed = 10;
    [SerializeField] private int bounceCount = 2;
    [SerializeField] private float bounceGravity;

    [Header("Pierce Info")]
    [SerializeField] private int pierceCount = 2;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float damageCD = .35f;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float maxTravelDistance = 10;
    [SerializeField] private float spinGravity = 1;

    protected override void Start()
    {
        base.Start();
        //先生成点
        GenerateDots();

        //根据剑的种类设置重力
        
    }

    private void SetUpGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
            default:
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
        SetUpGravity();
        //用一个 is 语句来判断当前状态是否是 AimSwordState从而避免 bug
        if (Input.GetKey(KeyCode.F) && player.StateMachine.currentState is PlayerAimSwordState)
        {
            DotsActive(true);
            for (int i = 0; i < dotsCount; i++)
            {
                dots[i].transform.position = DotPosition(i * dotSpacing);
            }
        }

    }


    public override void UseSkill()
    {
        base.UseSkill();
        //最终方向为鼠标方向乘以发射力
        finalDir = new Vector2(AimDir().x * launchForce, AimDir().y * launchForce);
        GameObject sword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SwordSkillController sSCtrl = sword.GetComponent<SwordSkillController>();

        if(swordType is SwordType.Bounce)
        {
            sSCtrl.SetUpBounce(isBouncing: true, bounceCount, boucingSpeed);
        }
        else if (swordType is SwordType.Pierce)
        {
            sSCtrl.SetUpPierce(pierceCount);
        }
        else if (swordType is SwordType.Spin)
        {
            sSCtrl.SetUpSpin(maxTravelDistance, spinDuration, isSpinning: true, damageCD);
        }
        player.AssignNewSword(sword);
        sSCtrl.SetUpSword(finalDir, swordGravity, player, freezeTime, returnSpeed);

        //放出后轨迹消失
        DotsActive(false);
    }

    //通过屏幕鼠标确定发射方向
    private Vector2 AimDir()
    {
        //ScreenToWorldPoint将屏幕坐标转换为世界坐标
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;
        Vector2 dir = (mousePos - playerPos).normalized;
        return dir;
    }


    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[dotsCount];
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotPosition(float t)
    {
        finalDir = new Vector2(AimDir().x * launchForce, AimDir().y * launchForce);
        //Physics.gravity 返回的是一个 Vector2D，其中 x 分量是 0，y 分量是 -9.81，表示重力加速度。
        Vector2 positon = (Vector2)player.transform.position + finalDir * t + 0.5f * Physics2D.gravity * swordGravity * t * t;
        return positon;
    }


}
