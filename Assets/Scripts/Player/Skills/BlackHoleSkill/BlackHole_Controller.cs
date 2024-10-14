using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlackHole_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPrefab;
    private float growSpeed;
    private float maxRadius;
    private float shrinkSpeed;

    private bool canGrow = true;
    private bool canShrink;
    //初始化，否则会报错
    public List<KeyCode> keys;//准备好的KeyCodes
    private List<Transform> enemyTargets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();//玩家按下按键后加入的KeyCode对象

    private bool canCreateHotKeys = true; //专门控制后面进入的没法生成热键
    private bool cloneAttackReleased;
    private int amountOfAttacks;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private float BlackHoleDuration;

    public void SetUpBlackHole(float maxRadius, float growSpeed, float shrinkSpeed, int amountOfAttacks, float cloneAttackCD, float blackHoleDuration)
    {
        this.maxRadius = maxRadius;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCD;
        this.BlackHoleDuration = blackHoleDuration;
    }

    // Update is called once per frame
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        BlackHoleDuration -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.R) || BlackHoleDuration <= 0)
        {
            cloneAttackReleased = true;
            canCreateHotKeys = false;
            DestroyHotKeys();
            PlayerManager.instance.player.MakeTransparent(true);
        }

        CloneAttackLogic();

        BlackHoleLogic();
    }

    private void BlackHoleLogic()
    {
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxRadius, maxRadius), Time.deltaTime * growSpeed);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.1f)
            {
                Destroy(gameObject);
                canShrink = false;
            }
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackReleased && cloneAttackTimer <= 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = UnityEngine.Random.Range(0, enemyTargets.Count);

            //攻击偏移
            float cloneAttackOffset = 0;

            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                cloneAttackOffset = 2;
            }
            else
            {
                cloneAttackOffset = -2;
            }

            if (enemyTargets.Count > 0 && amountOfAttacks > 0)
            {
                SkillManager.instance.clone.CreateClone(enemyTargets[randomIndex], cloneAttackOffset);
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0 || BlackHoleDuration <= 0)
            {
                ExitController();
            }
        }
    }

    private void ExitController()
    {
        canShrink = true;
        cloneAttackReleased = false;
        PlayerManager.instance.player.Invoke("ExitBlackHoleState", 0.7f);
        PlayerManager.instance.player.CanBlackHoleReleased(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.FreezeTime(true);
            CreateHotKey(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);
    //{
    //    if (collision.GetComponent<Enemy>() != null)
    //    {
    //        collision.GetComponent<Enemy>().FreezeTime(false);
    //    }
    //}
    private void CreateHotKey(Collider2D collider)
    {
        if (keys.Count == 0)
        {
            return;
        }
        if (!canCreateHotKeys)
        {
            return;
        }

        if (keys.Count <= 0)
        {
            //警告玩家没有足够的按键
            Debug.LogWarning("Not enough keys");
            return;
        }

        GameObject hotKey = Instantiate(hotKeyPrefab, new Vector2(collider.transform.position.x, collider.transform.position.y + 2), Quaternion.identity);
        createdHotKey.Add(hotKey);

        KeyCode keyCode = keys[UnityEngine.Random.Range(0, keys.Count)];
        hotKey.GetComponent<BlackHole_HotKey_Controller>().SetUpHotKey(keyCode, collider.transform, this);
        keys.Remove(keyCode);
    }

    public void enemyTargetsAdd(Transform enemyTransform) => enemyTargets.Add(enemyTransform);

    private void DestroyHotKeys()
    {
        if(createdHotKey.Count <= 0)
        {
            return;
        }   

        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
}
