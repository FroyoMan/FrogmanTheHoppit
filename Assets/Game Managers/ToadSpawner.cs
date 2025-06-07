using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class ToadSpawner : MonoBehaviour
{
    [SerializeField]
    float minSpawnX, maxSpawnX, minSpawnY, maxSpawnY;

    [SerializeField]
    float x, y, radX, radY;
    [SerializeField]
    GameObject debug;
    [SerializeField] GameObject toad;
    [SerializeField]
    Collider2D toadSpawner, spawnChecker;

    private float i;
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        while (i < 100)
        {
            i++;
            float numb = UnityEngine.Random.Range(-4.15f, 5.15f);
            spawnChecker.transform.position = new UnityEngine.Vector2(numb, 4.5f);
            // bool over = Physics2D.OverlapBox(new UnityEngine.Vector2(numb, 4.5f), new UnityEngine.Vector2(radX, radY), 0f, LayerMask.GetMask("EnemySpawn"));
            if (toadSpawner.bounds.Contains(spawnChecker.bounds.min) && toadSpawner.bounds.Contains(spawnChecker.bounds.max))
            {
                Instantiate(toad, new UnityEngine.Vector2(numb, 4.5f), quaternion.identity);
            }
            else
            {
                continue;
            }

        }
        debug.transform.position = new UnityEngine.Vector2(x, y);
        debug.transform.localScale = new UnityEngine.Vector2(radX, radY);
        // bool temp = Physics2D.OverlapBox(new UnityEngine.Vector2(x, y), new UnityEngine.Vector2(radX, radY), 0f, LayerMask.GetMask("Player"));
        // Debug.Log(temp);


    }
}
