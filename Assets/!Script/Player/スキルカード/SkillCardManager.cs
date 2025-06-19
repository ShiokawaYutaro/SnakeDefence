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

    public bool skillSelect = true;
    public int lvlCount = 1;

    private void Start()
    {
        instance = this;
        lvlCount = 0;
        // DrawCard();
    }

    public void StartDraw()
    {
        StartCoroutine(DrawCard());
    }
    private IEnumerator DrawCard()
    {
        if (!skillSelect)
        {
            lvlCount++;
            yield break;
        }

        cardList = new List<SkillCard>(2);
        for (int i = 0; i < 2; i++)
        {
            cardList.Add(Instantiate(skillCardPrefab, cardPlace).GetComponent<SkillCard>());

            int rand = Random.Range(0, skillCardDataList.Count);
            cardList[i].Initialize(skillCardDataList[rand]);
        }

        skillSelect = false;
    }



    public static void DeleteCard()
    {
        cardList.Clear();
    }
}
