using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{

    private SpriteRenderer sr;

    [Header("Flash FX")]
    public float flashTime = 0.1f;
    [SerializeField]private Material hitMat;
    private Material originMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] ignitColor;
    [SerializeField] private Color[] shockColor;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashTime);

        sr.color = currentColor;
        sr.material = originMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.red)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public void MakeTransparent(bool beTransparent)
    {
        if (beTransparent)
        {
            sr.color = Color.clear;
        }
        else { sr.color = Color.white; }
    }
    public void IgniteFxFor(float _seconds)
    {
        Debug.Log("ignite ignite ignite");
        InvokeRepeating("IgniteColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }



    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void IgniteColorFx()
    {
        if(sr.color != ignitColor[0])
        {
            sr.color = ignitColor[0];
        }
        else
        {
            sr.color = ignitColor[1];
        }
    }


    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    private void ChillColor()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }


}
