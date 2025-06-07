using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerGaugeManager : MonoBehaviour
{
    [SerializeField] Image power;
    private float amount = 0;
    private bool hasFlashed = false, isShooting = false;
    [SerializeField] PlayerBarFadeout pbf;
    [SerializeField] GameObject laser;
    [SerializeField] playerController pc;
    Coroutine flashCoroutine;

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartLaser();
        }
    }

    private void Update()
    {
        power.fillAmount = amount;
        if (!hasFlashed)
        {
            amount += (float)1 / 60 * Time.deltaTime;
            // amount += 1 * Time.deltaTime;

            if (amount >= 1)
            {
                power.fillAmount = 1f;
                hasFlashed = true;
                flashCoroutine = StartCoroutine(pbf.FlashGauge());
            }
        }

        if (isShooting)
        {
            amount -= 0.25f * Time.deltaTime;
            if (laser.transform.localScale.x < 1)
            {
                UnityEngine.Vector2 temp = laser.transform.localScale;
                temp.x = temp.x + 2 * Time.deltaTime;
                laser.transform.localScale = temp;
            }
        }
        else
        {
            UnityEngine.Vector2 temp = laser.transform.localScale;
            temp.x = temp.x - 2 * Time.deltaTime;
            if (temp.x < 0)
            {
                temp.x = 0;
            }
            laser.transform.localScale = temp;
        }
    }

    public void AddAmount(float add)
    {
        amount += add;
    }

    public void StartLaser()
    {
        if (amount >= 1f)
        {
            StartCoroutine(ShootLaser());
            StopCoroutine(flashCoroutine);
            pbf.Reset();
        }
    }
    public IEnumerator ShootLaser()
    {
        isShooting = true;
        pc.SetIsLasering(true);
        while (amount > 0)
        {
            yield return null;
        }
        isShooting = false;
        pc.SetIsLasering(false);
        hasFlashed = false;
        yield return null;
    }


    public void ResetFlash()
    {
        hasFlashed = false;
    }
}
