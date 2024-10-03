using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cC;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cC = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 flyDir, float gravity)
    {
        rb.velocity = flyDir;
        rb.gravityScale = gravity;
    }

    
}
