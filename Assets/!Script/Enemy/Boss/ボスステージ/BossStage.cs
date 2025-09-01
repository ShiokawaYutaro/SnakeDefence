using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    [SerializeField] GameObject nextStage;
    static public bool startAction;
    [SerializeField] Boss prefabBoss;
    Transform cameraPos;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (startAction) return;
            startAction = true;
            nextStage.SetActive(true);
            var boss = Instantiate(prefabBoss ,transform.position,Quaternion.identity);

        }
    }
}
