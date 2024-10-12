using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotKeyPrefab;
    public List<KeyCode> keys;

    [SerializeField] private float growSpeed;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canGrow;
    //初始化，否则会报错
    private List<Transform> enemyTargets = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxRadius, maxRadius), Time.deltaTime * growSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Enemy>() != null)
        {
            CreateHotKey(collider);
        }
    }

    private void CreateHotKey(Collider2D collider)
    {
        if (keys.Count <= 0)
        {
            //警告玩家没有足够的按键
            Debug.LogWarning("Not enough keys");
            return;
        }

        Enemy enemy = collider.GetComponent<Enemy>();
        enemy.FreezeTime(true);

        GameObject hotKey = Instantiate(hotKeyPrefab, new Vector2(collider.transform.position.x, collider.transform.position.y + 2), Quaternion.identity);
        KeyCode keyCode = keys[Random.Range(0, keys.Count)];
        hotKey.GetComponent<BlackHole_HotKey_Controller>().SetUpHotKey(keyCode, collider.transform, this);
        keys.Remove(keyCode);
    }

    public void enemyTargetsAdd(Transform enemyTransform) => enemyTargets.Add(enemyTransform);
}
