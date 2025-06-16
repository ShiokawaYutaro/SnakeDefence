using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] Enemy prefab;
    public float spawnInterval;
    float spawnTime;

    private void FixedUpdate()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime > spawnInterval)
        {
            spawnTime = 0;
            Instantiate(prefab,transform.position,Quaternion.identity);
        }
    }
}
