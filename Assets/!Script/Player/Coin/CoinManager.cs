using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    Player player;
    [SerializeField] Text coinText;

    private void Start()
    {
        instance = this;
        player = transform.root.Find("body").GetComponent<Player>();
    }
    private void Update()
    {
        coinText.text = player.coin.ToString("f0");
    }
    public void AddCoin(int _addCoin)
    {
        player.coin += _addCoin;
    }

}
