using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            Debug.Log("There are more than one SkillManager in the scene");
        }
        else
        {
            instance = this;
        }
    }
}
