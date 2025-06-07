using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerUI : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] playerHealth playerHealth;
    List<HeartBehaviorUI> hearts = new List<HeartBehaviorUI>();

    private void OnEnable()
    {
        playerHealth.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerDamaged -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        int heartsToMake = (int)playerHealth.GetMaxHP();
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.GetHP() - i, 0, 1);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform, false);

        HeartBehaviorUI heartComponent = newHeart.GetComponent<HeartBehaviorUI>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartBehaviorUI>();
    }
}
