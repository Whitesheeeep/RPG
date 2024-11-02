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
        //�����ɵ�
        GenerateDots();

        //���ݽ���������������
        
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
        //��һ�� is ������жϵ�ǰ״̬�Ƿ��� AimSwordState�Ӷ����� bug
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
        //���շ���Ϊ��귽����Է�����
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
        finalDir = new Vector2(AimDir().x * launchForce, AimDir().y * launchForce);
        //Physics.gravity ���ص���һ�� Vector2D������ x ������ 0��y ������ -9.81����ʾ�������ٶȡ�
        Vector2 positon = (Vector2)player.transform.position + finalDir * t + 0.5f * Physics2D.gravity * swordGravity * t * t;
        return positon;
    }


}
