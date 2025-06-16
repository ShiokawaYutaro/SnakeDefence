using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardManager : MonoBehaviour
{
    public static SkillCardManager instance;
    [SerializeField] SkillCard skillCardPrefab;
    [SerializeField] List<SkillCardData> skillCardDataList;

    [SerializeField] Transform cardPlace;
    public static List<SkillCard> cardList = null;

    public bool skillSelect;

    private void Start()
    {
        instance = this;
       // DrawCard();
    }
    public void DrawCard()
    {
        if (!skillSelect) return;

        cardList = new List<SkillCard>(2);
        for (int i = 0; i < 2; i++)
        {
            cardList.Add (Instantiate(skillCardPrefab, cardPlace));

            int rand = Random.Range(0, skillCardDataList.Count);
            cardList[i].Initialize(skillCardDataList[rand]);
        }
        
    }
    public static void DeleteCard()
    {
        cardList.Clear();
    }
}
