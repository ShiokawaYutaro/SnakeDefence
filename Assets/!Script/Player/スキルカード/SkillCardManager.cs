using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardManager : MonoBehaviour
{
    [SerializeField] SkillCard skillCardPrefab;
    [SerializeField] List<SkillCardData> skillCardDataList;

    [SerializeField] Transform cardPlace;
    public static List<SkillCard> cardList = null;

    

    private void Start()
    {
        DrawCard();
    }
    public void DrawCard()
    {
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
