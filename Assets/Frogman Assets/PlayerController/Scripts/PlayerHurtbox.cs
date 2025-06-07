using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField]
    float invTime, boost;
    [SerializeField] FlashScript flashEffect;
    [SerializeField] Collider2D hurtBox;
    [SerializeField] CameraShake cameraScript;
    private float timeTilHit;
    [SerializeField] playerHealth player;
    [SerializeField] playerController playerController;

    private void Update()
    {
        timeTilHit -= Time.deltaTime;
        if (timeTilHit <= 0)
        {
            hurtBox.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 12) && timeTilHit <= 0)
        {
            cameraScript.TriggerShake(0.2f, 0.1f);
            player.TakeDamage(1);
            timeTilHit = invTime;
            flashEffect.Flash();
            playerController.GiveSpeedBoost(boost, invTime);
            hurtBox.enabled = false;
        }
    }
}
