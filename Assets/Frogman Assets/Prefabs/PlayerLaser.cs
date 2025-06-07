using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("colliding with" + collision.gameObject);
        if (collision.gameObject.layer == 9)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }


}
