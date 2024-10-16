using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;


    #region ��������
    [SerializeField] private float crystalSkillDuration;

    private int pressQCount = 0;

    //��ˮ��������ˮ�������ڷ���
    [SerializeField] private bool swapInsteadOfCrystal;

    [Header("Crystal Explode Info")]
    [SerializeField] private bool canExplode;//�����ܷ�ը
    [SerializeField] private float maxExplosionRadius;
    [SerializeField] private float explosionSpeed = 1f;

    [Header("Crystal Trace Info")]
    [SerializeField] private bool canTrace;
    [SerializeField] private float traceSpeed;

    [SerializeField] private bool canSwap;

    [Header("MultiCrystal Info")]
    [SerializeField] private bool canMultiCrystal;
    public List<GameObject> crystalList = new List<GameObject>();
    [SerializeField] private int canProducedCrystalCount = 3;
    private int usedCrystalCount = 0;
    private float multiCrystalDuration;
    private float tempCoolDownDuration;

    /// <summary>
    /// ��playerֻ���˲���ˮ��ʱ��������ˮ����ʱ��
    /// </summary>
    [SerializeField] private float timeOfFillDuration;
    private float timerOfFillDuration;
    //[SerializeField] private float fillCrystalTimer;

    #endregion
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void Update()
    {
        base.Update();
        if (!canMultiCrystal)
        {
            if (multiCrystalDuration > 0)
            {
                multiCrystalDuration -= Time.deltaTime;
            }
            if (timerOfFillDuration > 0)
            {
                timerOfFillDuration -= Time.deltaTime;
            }
            if (multiCrystalDuration <= 0)
            {
                ResetMultiCrystal();
            } 
        }
    }

    protected override void Start()
    {
        base.Start();
        tempCoolDownDuration = cooldown;
        FillMultiCrystal();
        
    }
    public override void UseSkill()
    {
        base.UseSkill();
        if (!canMultiCrystal)
        {
            pressQCount++;
            if (pressQCount < 2)
            {
                cooldown = 0;
            }
            else
            {
                cooldown = tempCoolDownDuration;
                pressQCount = 0;
            } 
        }


        #region ���ɶ��ˮ�������������������ֶ������Լ���λ
        if (canMultiCrystal)
        {
            if (multiCrystalDuration > 0)
            {
                return;
            }
            cooldown = 0;
            if(crystalList.Count > 0)
            {
                GameObject crystal = crystalList[crystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystal, player.transform.position, Quaternion.identity);
                newCrystal.GetComponent<CrystalController>().SetUpCrystal(crystalSkillDuration,
                    canExplode, maxExplosionRadius, explosionSpeed,
                    canTrace, traceSpeed);
                crystalList.Remove(crystal);
                usedCrystalCount++;
                ResetNotFullFillTimer();
                if (crystalList.Count <= 0)
                {
                    FillMultiCrystal();

                }
            }

            if(usedCrystalCount == canProducedCrystalCount)
            {
                multiCrystalDuration = tempCoolDownDuration;
                usedCrystalCount = 0;
                return;
            }
            return;
        }
        #endregion ���ɶ��ˮ��

        
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            var crystalController = currentCrystal.GetComponent<CrystalController>();
            crystalController.SetUpCrystal(crystalSkillDuration,
                canExplode,maxExplosionRadius,explosionSpeed,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                canTrace, traceSpeed);
        }
        else
        {
            //canTrance���ٴΰ��±�ը
            if (canTrace && canExplode)
            {
                currentCrystal.GetComponent<CrystalController>().DurationTobeZero();
                return;
            }

            #region �����߼���������Խ�������ô����λ�ã��������ٵ�ǰˮ��

            if (canSwap)
            {
                var temPosition = player.transform.position;    
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = temPosition;
                if (swapInsteadOfCrystal)
                {
                    SkillManager.instance.clone.CreateClone(currentCrystal.transform);
                    Destroy(currentCrystal);
                }
            }
            //�ɵ�ˮ����λ�ã����ǽ���
            else
            {
                player.transform.position = currentCrystal.transform.position;
                Destroy(currentCrystal);
            }
            #endregion �����߼�
        }
        
    }

    private void FillMultiCrystal()
    {
        for (int i = 0; i < canProducedCrystalCount; i++)
        {

            crystalList.Add(crystalPrefab);

        }
    }

    private void ResetNotFullFillTimer() => timerOfFillDuration = timeOfFillDuration;

    private void ResetMultiCrystal()
    {
        if (timerOfFillDuration <= 0 && usedCrystalCount != 0)
        {

            for (int i = 0; i < usedCrystalCount; i++)
            {
                crystalList.Add(crystalPrefab);
            }
            usedCrystalCount = 0;  
        }
    }
}
