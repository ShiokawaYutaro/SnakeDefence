using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] Enemy prefab;
    public float spawnInterval;
    float spawnTime;
    private List<Enemy> enemyList = new List<Enemy>();

    private void FixedUpdate()
    {
        if (enemyList.Count >= 8) return;

        spawnTime += Time.deltaTime;
        if(spawnTime > spawnInterval)
        {
            spawnTime = 0;
            Enemy enemy = Instantiate(prefab,transform.position,Quaternion.identity);
            enemyList.Add(enemy);
            enemy.enemySpawn = this;
        }
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
}
