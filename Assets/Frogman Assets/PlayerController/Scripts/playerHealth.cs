using System;
using System.ComponentModel;
using UnityEngine;

public class playerHealth : MonoBehaviour
{

    [SerializeField]
    float maxHp;
    private float hp;
    public static event Action OnPlayerDamaged;
    [SerializeField] PlayerBarFadeout pbf;


    private void Awake()
    {
        hp = maxHp;
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        OnPlayerDamaged?.Invoke();
        StartCoroutine(pbf.ShowHeart());
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public float GetHP()
    {
        return hp;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }

}
