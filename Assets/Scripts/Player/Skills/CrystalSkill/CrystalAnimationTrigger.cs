using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimationTrigger : MonoBehaviour
{
    public void CrystalExplosionFinish() => Destroy(transform.parent.gameObject);

    public void CrystalExplosion() => transform.parent.GetComponent<CrystalController>().Explode();
}
