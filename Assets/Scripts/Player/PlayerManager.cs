using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(99)]
public class PlayerManager : MonoBehaviour
{
    //单例模式
    public static PlayerManager instance;
    public Player player;

    public int currency;

    
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

    public bool HaveEnoughMoney(int _price)
    {
        if(_price > currency)
        {
            Debug.Log("你没有足够的资源");
            return false;
        }

        currency -= _price;
        return true;
    }
}
