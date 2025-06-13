using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    Image image;
    public void Initialize(SkillCardData data)
    {
        image = GetComponent<Image>();
        image.sprite = data.icon;
    }

    public void OnClick()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }
        SkillCardManager.DeleteCard();
    }
}
