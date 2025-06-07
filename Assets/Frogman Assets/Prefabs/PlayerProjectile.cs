using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Animator ani;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] PowerGaugeManager pgm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = speed;
        pgm = FindAnyObjectByType<PowerGaugeManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            pgm.AddAmount(0.01f);
            rb.linearVelocityY = 0;
            ani.SetBool("isHit", true);
            collision.GetComponent<Enemy>().TakeDamage(damage);
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }


}
