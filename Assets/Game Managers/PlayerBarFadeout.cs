using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerBarFadeout : MonoBehaviour
{
    private HashSet<Collider2D> objectsInTrigger = new HashSet<Collider2D>();
    [SerializeField] CanvasGroup hpGroup, gaugeGroup;
    [SerializeField] Image power;
    private Material originalMaterial;
    [SerializeField] Material flashMaterial;
    [SerializeField] float fadeSpeed;
    [SerializeField] float flashDuration, flashAmount;
    [SerializeField] bool isFlashing;

    private void Start()
    {
        originalMaterial = power.material;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            return;
        }
        if (other)
        {
            objectsInTrigger.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other)
        {
            objectsInTrigger.Remove(other);
        }
    }

    private void Update()
    {
        // Do something if at least one object is still in the trigger
        if (objectsInTrigger.Count > 0 && hpGroup.alpha > 0.3f && gaugeGroup.alpha > 0.3f && !isFlashing)
        {
            float a = hpGroup.alpha - fadeSpeed * Time.deltaTime;
            if (a < 0.3)
            {
                hpGroup.alpha = 0.3f;
                gaugeGroup.alpha = 0.3f;
            }
            else
            {
                hpGroup.alpha = a;
                gaugeGroup.alpha = a;
            }

        }
        else
        {
            hpGroup.alpha += fadeSpeed * Time.deltaTime;
            gaugeGroup.alpha += fadeSpeed * Time.deltaTime;
        }
    }

    public IEnumerator FlashGauge()
    {
        isFlashing = true;
        for (int i = 0; i < (int)flashAmount; i++)
        {
            power.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            power.material = originalMaterial;
            yield return new WaitForSeconds(flashDuration);
        }
        isFlashing = false;
        yield return null;
    }

    public IEnumerator ShowHeart()
    {
        isFlashing = true;
        for (int i = 0; i < (int)flashAmount; i++)
        {
            yield return new WaitForSeconds(flashDuration * 2);
        }
        isFlashing = false;
        yield return null;
    }

    public void Reset()
    {
        power.material = originalMaterial;
        isFlashing = false;
    }
}
