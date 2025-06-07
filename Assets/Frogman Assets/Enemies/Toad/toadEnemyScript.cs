using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class toadEnemyScript : Enemy
{
    private float elapsed = 0;
    private float cooldownTimer = 0;
    private Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField]
    float distance, duration, coolDown, health;
    private UnityEngine.Vector2 endPosition;
    private UnityEngine.Vector2 startPosition;
    private UnityEngine.Vector2 lerpy;
    void Start()
    {
        //Add a script that randomly decides this toad's speed/AI
        rb = GetComponent<Rigidbody2D>();
        SetHealth(health);
        UpdatePositions();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (cooldownTimer <= 0)
        {
            float percent = elapsed / duration;
            if (percent > 1)
            {
                animator.SetFloat("isMoving", 0);
                elapsed = 0;
                UpdatePositions();
            }
            else
            {
                animator.SetFloat("isMoving", 1);
                lerpy = UnityEngine.Vector2.Lerp(startPosition, startPosition - endPosition, percent);
                rb.MovePosition(lerpy);
                elapsed += Time.deltaTime;
            }
        }
        else
        {
            cooldownTimer -= +Time.deltaTime;
        }
    }

    private void UpdatePositions()
    {
        startPosition = rb.position;
        endPosition = new UnityEngine.Vector2(0, distance); //.5 1
        cooldownTimer = coolDown; // -0.5. 0.5 +2
    }

    public void DisableMovement()
    {
        rb.simulated = false;
    }

}