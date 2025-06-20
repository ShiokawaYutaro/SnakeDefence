using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static int coin;

    [SerializeField] Text coinText;

    private void Update()
    {
        coinText.text = coin.ToString("f0");
    }
    public static void AddCoin(int _addCoin)
    {
        coin += _addCoin;
    }

}
