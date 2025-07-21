using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coin;
    public static CoinManager instance;

    [SerializeField] Text coinText;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        coinText.text = coin.ToString("f0");
    }
    public void AddCoin(int _addCoin)
    {
        coin += _addCoin;
    }

}
