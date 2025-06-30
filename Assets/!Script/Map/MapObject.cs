using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapObject : MonoBehaviour
{
    [SerializeField] List<GameObject> MapAppearPoint = null;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Stage");
        }
    }
}
