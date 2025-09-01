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
        transform.Find("Canvas/Icon").GetComponent<Image>().sprite = Icon;
        //Icon.sprite = data.icon;
        //cardName = data.cardName;
       // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        transform.Find("Canvas/Icon").GetComponent<Image>().sprite = Icon;
        //Icon.sprite = data.icon;
        //cardName = data.cardName;
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Update()
    {
        float sin = Mathf.Sin(Time.time);
        this.transform.position = new Vector3(transform.position.x, transform.position.y + sin * 0.001f, transform.position.z);
    }

    private GameObject perkCard; // 生成したカードを保持

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponentInParent<Player>();

            // まだ生成していなければ生成
            if (perkCard == null)
            {
                perkCard = Instantiate(
                    perkUI,
                    player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform
                );

                perkCard.GetComponentInChildren<Button>().onClick.AddListener(Buy);
                perkCard.transform.Find("Icon").GetComponent<Image>().sprite = Icon;
                perkCard.transform.Find("Name").GetComponent<Text>().text = perkName;
            }

            // 色の更新だけは毎フレーム行う
            if (player.coin < 10)
                perkCard.GetComponent<Image>().color = Color.red;
            else
                perkCard.GetComponent<Image>().color = new Color(0f, 1f, 0.22f); // 0-1範囲に修正
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.GetComponentInParent<Player>();
            onViews = false;
            Destroy(player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform.GetChild(0).gameObject);
        }
            
    }

    private void Buy()
    {
        if (player.coin < 10)
        {
            player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").transform.GetChild(0).DOShakePosition(1f, 20);
            SEManager.Instance.NoCanBuySE();
            return;
        }

        player.coin -= 10;

        GameObject perkCard = player.transform.parent.Find("ゲーム画面/ボタン関係/パーク選択").GetChild(0).gameObject;
        Text cardText = perkCard.transform.Find("Name").GetComponent<Text>();
        SEManager.Instance.CanBuySE();

        if (cardText.text == "攻撃力") { player.SetPower(5); }
        if(cardText.text == "防御力") { player.SetDefence(5); }
        if(cardText.text == "HP") { player.SetMaxHP(50); }
        //if(cardText.text == "Coin") { player.; }

    }
}
