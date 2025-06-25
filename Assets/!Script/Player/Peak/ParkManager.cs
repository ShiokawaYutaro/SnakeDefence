using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkManager : MonoBehaviour
{
    public static ParkManager instance;
    [SerializeField] SkillCard skillCardPrefab;
    [SerializeField] List<SkillCardData> skillCardDataList;

    public static List<SkillCard> cardList = null;

    public bool skillSelect = true;
    public int lvlCount = 1;

    private void Start()
    {
        instance = this;
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
            //cardList.Add(Instantiate(skillCardPrefab, cardPlace).GetComponent<SkillCard>());

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
