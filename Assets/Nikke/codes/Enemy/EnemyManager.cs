using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    private List<GameObject> enemyList = new List<GameObject>();

    public GameObject enemyPrefab;

    private void Awake()
    {
        if (enemyManager == null)
        {
            enemyManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnEnemy(new Vector3(-5, 2.25f, 0));
        SpawnEnemy(new Vector3(0, 2.25f, 0));
        SpawnEnemy(new Vector3(5, 2.25f, 0));

    }
    void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemyList.Add(enemy);
    }



}
