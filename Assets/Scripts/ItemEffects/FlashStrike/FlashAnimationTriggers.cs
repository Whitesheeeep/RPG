using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashAnimationTriggers : MonoBehaviour
{
    public void DestroyEffect()
    {
        GetComponentInParent<FlashAttack_Controller>().DestroyEffect();
    }

}
