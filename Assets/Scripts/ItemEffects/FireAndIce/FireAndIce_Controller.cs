using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAndIce_Controller : FlashAttack_Controller
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb; 
    private Player player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player;
        rb.velocity = new Vector2(speed * player.facingDir, 0);
    }

    private void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
