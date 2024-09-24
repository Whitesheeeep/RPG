using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    public float flashTime = 0.1f;

    private SpriteRenderer sr;
    [SerializeField]private Material hitMat;
    private Material originMat;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashTime);
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

    private void WhiteFinishBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
