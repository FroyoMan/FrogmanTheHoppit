using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Toadster : Enemy
{
    private Rigidbody2D rb;

    #region 
    [Header("Animation Times")]
    [SerializeField] float jumpReadyTime, jumpReadyTime2;
    #endregion
    [SerializeField] float spriteJumpSpeed;
    [SerializeField] float setHealth, jumpTimer, jumpDSpeed, jumpDuration, airTime, fallDuration;
    [SerializeField] float attackReadyTime, attackingTime, attackWaitingTime;
    [SerializeField] Animator animator;
    [SerializeField] playerController player;
    [SerializeField] GameObject hurtbox, sprite, tongueGameObject, tongueParentGameObject;
    [SerializeField] Collider2D poly;
    private UnityEngine.Vector2 startPosition, lerpy, endPosition;
    private float elapsedTime, percent;
    [SerializeField] Coroutine jumpCoroutine;
    [SerializeField] SpriteRenderer shadow;
    [SerializeField] CameraShake cameraShakeScript;
    #region 
    [Header("Tongue")]
    [SerializeField] float actionTimer;
    [SerializeField] int cheatChoice = -1;
    [SerializeField] int previousChoice = 1;
    private Coroutine tongueCoroutine;
    private Coroutine tongueThenJumpCoroutine;
    private bool isFirstMove = true;
    #endregion
    void Start()
    {
        //Add a script that randomly decides this toad's speed/AI
        startPosition = this.transform.position;
        rb = GetComponent<Rigidbody2D>();
        SetHealth(setHealth);
        Boss();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GetHealth() < 100)
        {
            actionTimer = 1;
        }

        if (jumpTimer < 0 && jumpCoroutine == null && tongueCoroutine == null && tongueThenJumpCoroutine == null)
        {
            int choice = UnityEngine.Random.Range(0, 5);
            while (choice == previousChoice)
            {
                choice = UnityEngine.Random.Range(0, 5);

            }

            while (isFirstMove && (choice == 1 || choice == 4))
            {
                choice = UnityEngine.Random.Range(0, 5);
            }

            if (cheatChoice != -1)
            {
                choice = cheatChoice;
            }
            previousChoice = choice;
            switch (choice)
            {
                case 0:
                    jumpCoroutine = StartCoroutine(JumpPlayer(false, 1));
                    break;
                case 1:
                    tongueCoroutine = StartCoroutine(TonguePlayer());
                    break;
                case 2:
                    jumpCoroutine = StartCoroutine(JumpPlayer(false, UnityEngine.Random.Range(2, 4)));
                    break;
                case 3:
                    tongueThenJumpCoroutine = StartCoroutine(JumpThenTonguePlayer());
                    break;
                case 4:
                    tongueThenJumpCoroutine = StartCoroutine(TongueThenJumpPlayer());
                    break;
                default:
                    break;
            }

        }
        jumpTimer -= Time.deltaTime;

    }

    IEnumerator JumpPlayer(bool tongue, float jumpCounts = 1, bool noResetAni = false)
    {
        elapsedTime = 0;
        percent = 0;

        Color c = shadow.color;
        float startAlpha = shadow.color.a;

        for (int i = 0; i < jumpCounts; i++)
        {
            float tempJRT;
            // GET READY TO JUMP //
            if (i != 0) { tempJRT = 0; }
            else { tempJRT = jumpReadyTime; }
            while (elapsedTime < tempJRT)
            {
                animator.SetBool("isJumpReady", true);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ResetAnimations();
            animator.SetBool("isJumping", true);
            ResetTime();
            hurtbox.SetActive(false);
            poly.enabled = false;
            Vector2 originalSpriteTransform = sprite.transform.localPosition;

            cameraShakeScript.TriggerShake(0.1f, 0.05f);
            // JUMPS
            while (elapsedTime < jumpDuration)
            {
                percent = elapsedTime / jumpDuration;
                // percent = Mathf.SmoothStep(0f, 1f, percent);
                percent = 1f - Mathf.Pow(1f - percent, 1.5f);
                sprite.transform.localPosition = Vector2.Lerp(originalSpriteTransform, new Vector2(originalSpriteTransform.x, originalSpriteTransform.y + jumpDSpeed), percent);
                c.a = Mathf.Lerp(startAlpha, 0.1f, percent);
                shadow.color = c;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ResetTime();


            endPosition = (Vector2)player.transform.position;
            if (UnityEngine.Random.value > 0.5f)
            {
                float[] input = player.GetComponent<playerController>().ReturnHorizontalAndVertial();
                endPosition = endPosition + new Vector2(input[0], input[1]).normalized * 3;
            }
            if (tongue == true) { endPosition.y = 2f; }

            endPosition.y = Mathf.Clamp(endPosition.y, -1.25f, 2f);
            endPosition.x = Mathf.Clamp(endPosition.x, -3.375f, 3.375f);

            // MOVES WHILE IN THE AIR
            while (elapsedTime < airTime)
            {
                elapsedTime += Time.deltaTime;
                percent = elapsedTime / airTime;
                percent = percent * percent * (3f - 2f * percent);
                lerpy = UnityEngine.Vector2.Lerp(startPosition, endPosition, percent);
                rb.MovePosition(lerpy);
                yield return null;
            }

            Vector2 airSpriteTransform = sprite.transform.localPosition;
            ResetTime();
            float endAlpha = c.a;
            // FALL BAACK DOWN
            while (elapsedTime < fallDuration)
            {
                percent = elapsedTime / fallDuration;
                percent = percent * percent;
                sprite.transform.localPosition = Vector2.Lerp(airSpriteTransform, originalSpriteTransform, percent);
                c.a = Mathf.Lerp(endAlpha, 1f, percent);
                shadow.color = c;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sprite.transform.localPosition = originalSpriteTransform;
            ResetTime();
            ResetAnimations();
            animator.SetBool("isJumpReady", true);
            hurtbox.SetActive(true);
            poly.enabled = true;
            hurtbox.SetActive(true);
            cameraShakeScript.TriggerShake(0.1f, 0.1f);

            // LANDS
            tempJRT = jumpReadyTime2;
            if (i >= jumpCounts - 1)
            {
                tempJRT = jumpReadyTime;
            }
            if (noResetAni)
            {
                tempJRT = 0;
            }


            while (elapsedTime < tempJRT)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            startPosition = transform.position;
            hurtbox.SetActive(false);
            ResetTime();

        }

        if (!noResetAni)
        {
            ResetAnimations();
        }

        jumpTimer = actionTimer;
        jumpCoroutine = null;
        startPosition = transform.position;
        yield return null;
    }

    IEnumerator TonguePlayer()
    {
        yield return jumpCoroutine = StartCoroutine(JumpPlayer(true));

        elapsedTime = 0;
        percent = 0;
        Vector2 originalScale = tongueGameObject.transform.localScale;
        animator.SetBool("isAttackReady", true);
        // GET READY TO ATTACK //
        while (elapsedTime < attackReadyTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetTime();
        ResetAnimations();
        animator.SetBool("isAttacking", true);
        tongueParentGameObject.SetActive(true);

        while (elapsedTime < attackingTime / 2)
        {
            percent = elapsedTime / attackingTime / 2;
            percent = 1f - Mathf.Pow(1f - percent, 1.5f);
            tongueGameObject.transform.localScale = Vector2.Lerp(originalScale, new Vector2(1, 16), percent);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < attackingTime)
        {
            percent = elapsedTime / attackingTime;
            percent = 1f - Mathf.Pow(1f - percent, 1.5f);
            tongueGameObject.transform.localScale = Vector2.Lerp(new Vector2(1, 16), originalScale, percent);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tongueParentGameObject.SetActive(false);
        ResetTime();
        ResetAnimations();
        animator.SetBool("isAttackReady", true);

        while (elapsedTime < attackWaitingTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetAnimations();
        ResetTime();

        jumpTimer = actionTimer;
        tongueCoroutine = null;
        yield return null;
    }

    IEnumerator TongueThenJumpPlayer(float jumpCounts = 1)
    {
        yield return tongueCoroutine = StartCoroutine(TonguePlayer());
        yield return jumpCoroutine = StartCoroutine(JumpPlayer(false, jumpCounts));


        jumpTimer = actionTimer;
        tongueThenJumpCoroutine = null;
        yield return null;
    }

    IEnumerator JumpThenTonguePlayer()
    {
        yield return jumpCoroutine = StartCoroutine(JumpPlayer(false, UnityEngine.Random.Range(1, 2), true));
        yield return tongueCoroutine = StartCoroutine(TonguePlayer());
        jumpTimer = actionTimer;
        tongueThenJumpCoroutine = null;
        yield return null;
    }

    private void ResetTime()
    {
        elapsedTime = 0;
        percent = 0;
    }
    private void ResetAnimations()
    {
        animator.SetBool("isJumpReady", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isPanting", false);
        animator.SetBool("isDisappearing", false);
        animator.SetBool("isAttackReady", false);
        animator.SetBool("isAttacking", false);
    }

    public void SetActionTimer(float t)
    {
        actionTimer = t;
    }


}