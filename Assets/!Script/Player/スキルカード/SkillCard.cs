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
        //���̖��O�̃X�L���Ȃ�
        if(cardName == "�|�C�Y��") { player.poison++; }
        if(cardName == "�t�@�C�A") { player.fire++; }
        if(cardName == "������") { player.regene++; }

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
