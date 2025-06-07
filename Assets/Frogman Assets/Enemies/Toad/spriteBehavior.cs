using UnityEngine;

public class spriteBehavior : MonoBehaviour
{
    public void Dead()
    {
        GetComponent<Animator>().SetBool("isDead", true);
    }

    public void DestroyEnemy()
    {
        GetComponentInParent<Enemy>().Destroy();

    }
}
