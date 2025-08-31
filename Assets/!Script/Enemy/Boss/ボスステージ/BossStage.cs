using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    static public bool startAction;
    [SerializeField] Boss prefabBoss;
    Transform cameraPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (startAction) return;
            startAction = true;

            var boss = Instantiate(prefabBoss ,transform.position,Quaternion.identity);

        }
    }
}
