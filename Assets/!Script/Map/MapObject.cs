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
            Player player = other.gameObject.GetComponent<Player>();

            PlayerSaveManager.SaveFromLobby(player.coin, player.power, player.defence, player.MaxHp);
            
            SceneManager.LoadScene("Stage");
        }
    }
}
