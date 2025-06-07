using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class playerController : MonoBehaviour
{
    #region 
    [Header("Player Settings")]
    [SerializeField] Animator animator, cloakAni;
    [SerializeField] GameObject cloak;

    private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    #endregion

    #region 
    [Header("Projectile Settings")]
    [SerializeField] float attackSpeed;
    [SerializeField] float shootingPointOffset;
    private float attackCooldown;
    [SerializeField] GameObject fireball;
    private bool isHoldingAttack, isOnCooldown = false, isLasering;
    #endregion

    private float boostTimer, speedBoost;
    private float horizontal, vertical;
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }
    public void Shoot(InputAction.CallbackContext context)
    {


        if (context.performed)
        {

            isHoldingAttack = true;
        }

        if (context.canceled)
        {
            isHoldingAttack = false;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {

        float speed = playerSpeed;
        boostTimer -= Time.deltaTime;
        if (!(boostTimer <= 0))
        {
            speed = playerSpeed + speedBoost;
        }
        else
        {
            speed = playerSpeed;
        }

        // rb.linearVelocity = new Vector2(horizontal * playerSpeed, vertical * playerSpeed);
        rb.MovePosition(rb.position + new Vector2(horizontal, vertical).normalized * speed * Time.deltaTime);

        animator.SetFloat("velocity", math.ceil(Math.Abs(horizontal) + Math.Abs(vertical)));
        if (horizontal < 0)
        {
            cloak.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            cloak.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (isHoldingAttack && !isOnCooldown && !isLasering)
        {
            attackCooldown = attackSpeed;
            Instantiate(fireball, transform.position + new Vector3(0f, shootingPointOffset, 0f), transform.rotation);
            isOnCooldown = true;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            isOnCooldown = false;
        }
    }

    public void GiveSpeedBoost(float boost, float time)
    {
        boostTimer = time;
        speedBoost = boost;
    }

    public void SetIsLasering(bool boo)
    {
        isLasering = boo;
    }

    public float[] ReturnHorizontalAndVertial()
    {
        float[] values = new float[] { horizontal, vertical };
        return values;
    }
}
