using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour {
    [SerializeField] Enemy enemyPrefab;
    private Player player;
    public int _enemyCount;

    [SerializeField] GameObject nextStage;

    public List<Enemy> enemyList = new List<Enemy>();

    bool onSpawn;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (enemyList.Count == 0 && onSpawn)
        {
            Destroy(gameObject);
        }

        if (nextStage == null) return;

        if(enemyList.Count > 0)
        {
            nextStage.SetActive(true);
        }
        else if(onSpawn)
        {
            nextStage.SetActive(false);
        }
        else
        {
            nextStage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onSpawn) return;
        if(other.gameObject.tag == "Player")
        {
            SummonEnemy();
            onSpawn = true;
        }
    }

    private void SummonEnemy()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 2f; // 半径2の円内
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y
            );
            Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.enemySpawn = this;
            enemyList.Add(enemy);

        }

    }

}
