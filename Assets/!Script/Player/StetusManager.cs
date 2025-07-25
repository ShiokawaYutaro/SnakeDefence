using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StetusManager : MonoBehaviour
{
    public static StetusManager instance;

    [SerializeField] public Text powerText;
    [SerializeField] public Text defenceText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetPowerText(Character character)
    {
        powerText.text = character.GetPower().ToString();
        defenceText.text = character.GetDefence().ToString();
    }
}
