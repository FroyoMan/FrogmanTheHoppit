using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private FlashScript flashEffect;
    [SerializeField] float BossFlashDamage;
    private float health, originalHealth, maxHealth;
    private bool isBoss = false;

    private void Start()
    {
        maxHealth = health;
    }
    public void TakeDamage(float damage)
    {


        health -= damage;
        if (health <= 0)
        {
            if (GetComponent<toadEnemyScript>() != null)
            {
                GetComponent<toadEnemyScript>().DisableMovement();
                GetComponentInChildren<spriteBehavior>().Dead();
            }
            else
            {
                Destroy();
            }

        }

        if (!isBoss)
        {
            flashEffect.Flash();
        }
        else if (originalHealth - BossFlashDamage >= health)
        {
            originalHealth = health;
            flashEffect.Flash();
        }
    }

    public void SetHealth(float hp)
    {
        health = hp;
        originalHealth = hp;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Destroy()
    {
        Destroy(gameObject);

    }

    public void Boss()
    {
        isBoss = !isBoss;
    }
}
