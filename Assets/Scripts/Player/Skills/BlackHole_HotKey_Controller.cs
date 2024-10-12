using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�����ı��ϵĽű�
public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private KeyCode myKeyCode;
    private SpriteRenderer sr;

    private Transform enemyTransform;
    private BlackHole_Controller blackHole_Controller;

    public void SetUpHotKey(KeyCode hitKyeCode, Transform enemyTransform, BlackHole_Controller blackHole_Controller)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();

        myKeyCode = hitKyeCode;
        myText.text = hitKyeCode.ToString();

        this.enemyTransform = enemyTransform;
        this.blackHole_Controller = blackHole_Controller;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(myKeyCode))
        {
            blackHole_Controller.enemyTargetsAdd(enemyTransform);

            //���°����󰴼���ʧ
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
