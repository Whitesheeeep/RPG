using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("SKill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;

    [Header("Dots Info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private int dotsCount;
    [SerializeField] private float dotSpacing;
    [SerializeField] private Transform dotsParent;

    private static GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        //先生成点
        GenerateDots();
    }


    protected override void Update()
    {
        base.Update();
        //用一个 is 语句来判断当前状态是否是 AimSwordState从而避免 bug
        if (Input.GetKey(KeyCode.F) && player.StateMachine.currentState is PlayerAimSwordState)
        {
            DotsActive(true);
            for (int i = 0; i< dotsCount; i++)
            {
                dots[i].transform.position = DotPosition(i * dotSpacing);
            }
        }
        
    }


    public override void UseSkill()
    {
        base.UseSkill();
        //最终方向为鼠标方向乘以发射力
        finalDir = new Vector2(AimDir().x * launchForce.x, AimDir().y * launchForce.y);
        GameObject sword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SwordSkillController sSCtrl = sword.GetComponent<SwordSkillController>();

        sSCtrl.SetUpSword(finalDir, swordGravity, player);

        player.AssignNewSword(sword);

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
        finalDir = new Vector2(AimDir().x * launchForce.x, AimDir().y * launchForce.y);
        //Physics.gravity 返回的是一个 Vector2D，其中 x 分量是 0，y 分量是 -9.81，表示重力加速度。
        Vector2 positon = (Vector2)player.transform.position + finalDir * t + 0.5f * Physics2D.gravity * swordGravity * t * t;
        return positon;
    }

    
}
