using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    Image image;
    string cardName;
    Player player;
    public void Initialize(SkillCardData data)
    {
        image = GetComponent<Image>();
        image.sprite = data.icon;
        cardName = data.cardName;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnClick()
    {
        //この名前のスキルなら
        if(cardName == "ポイズン") { player.poison++; }
        if(cardName == "ファイア") { player.fire++; }
        if(cardName == "持続回復") { player.regene++; }

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }
        SkillCardManager.instance.skillSelect = true;
        SkillCardManager.DeleteCard();
        
        if(SkillCardManager.instance.lvlCount >= 1)
        {
            
        }
        if (SkillCardManager.instance.lvlCount >= 1)
        {
            SkillCardManager.instance.StartDraw();
            SkillCardManager.instance.lvlCount--;

        }
    }
}
