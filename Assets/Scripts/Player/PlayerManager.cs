using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(99)]
public class PlayerManager : MonoBehaviour
{
    //µ¥ÀýÄ£Ê½
    public static PlayerManager instance;
    public Player player;

    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            Debug.Log("There are more than one PlayerManager in the scene");
        }
        else
        {
            instance = this;
        }
    }
}
