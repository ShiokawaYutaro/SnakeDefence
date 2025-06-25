using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    [SerializeField] private Sprite Icon;
    [SerializeField] string perkName;
    Player player;
    [SerializeField] GameObject perkUI;
    bool onViews;
    public void Initialize(SkillCardData data)
    {
        Icon = GetComponent<Sprite>();
        //Icon.sprite = data.icon;
        //cardName = data.cardName;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.Find("Canvas/Icon").GetComponent<Image>().sprite = Icon;
    }
    private void Update()
    {
        float sin = Mathf.Sin(Time.time);
        this.transform.position = new Vector3(transform.position.x, transform.position.y + sin * 0.001f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            if (onViews) return;
            onViews = true;
            GameObject perkCard = Instantiate(perkUI, player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform);
            perkCard.GetComponentInChildren<Button>().onClick.AddListener(Buy);
            perkCard.transform.Find("Icon").GetComponent<Image>().sprite = Icon;
            perkCard.transform.Find("Name").GetComponent<Text>().text = perkName;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            onViews = false;
            Destroy(player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform.GetChild(0).gameObject);
        }
            
    }

    private void Buy()
    {
        if (CoinManager.coin <= 10)
        {
            player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform.GetChild(0).DOShakePosition(1f,20);
            return;
        }

        CoinManager.coin -= 10;

    }
}
