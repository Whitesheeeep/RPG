using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        if (cam == null)
        {
            Debug.LogError("Main Camera not found");
        }

        xPosition = transform.position.x;//��ʼ�� xPosition
        
        //bounds ��ʾ��Ⱦ����Χ�е������size ��ʾ��Χ�еĳߴ磬size.x ��ʾ��Χ�еĿ��;https://docs.unity3d.com/cn/2020.2/ScriptReference/Renderer-bounds.html
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    /// <summary>
    ///����λ��
    /// </summary>
    void Update()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);//����������λ�����ľ���

        transform.position = new Vector3(xPosition + distance, transform.position.y);

        if (distanceMoved > xPosition + length) 
        {
            xPosition = xPosition + length;    
        }
        else if(distanceMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }

    }
}
