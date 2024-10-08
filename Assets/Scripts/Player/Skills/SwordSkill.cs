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
        //�����ɵ�
        GenerateDots();
    }


    protected override void Update()
    {
        base.Update();
        //��һ�� is ������жϵ�ǰ״̬�Ƿ��� AimSwordState�Ӷ����� bug
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
        //���շ���Ϊ��귽����Է�����
        finalDir = new Vector2(AimDir().x * launchForce.x, AimDir().y * launchForce.y);
        GameObject sword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SwordSkillController sSCtrl = sword.GetComponent<SwordSkillController>();

        sSCtrl.SetUpSword(finalDir, swordGravity, player);

        player.AssignNewSword(sword);

        //�ų���켣��ʧ
        DotsActive(false);
    }

    //ͨ����Ļ���ȷ�����䷽��
    private Vector2 AimDir()
    {
        //ScreenToWorldPoint����Ļ����ת��Ϊ��������
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
        //Physics.gravity ���ص���һ�� Vector2D������ x ������ 0��y ������ -9.81����ʾ�������ٶȡ�
        Vector2 positon = (Vector2)player.transform.position + finalDir * t + 0.5f * Physics2D.gravity * swordGravity * t * t;
        return positon;
    }

    
}
