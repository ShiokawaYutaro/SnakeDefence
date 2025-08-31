using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    bool menuOpen = false;

    Animation animation = null;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        if (!menuOpen)
        {
            animation.Play("Open");
            menuOpen = true;
            return;
        }
        else
        {
            animation.Play("Close");
            menuOpen = false;
            return;
        }
        
    }

    public void ExitGame()
    {
        Debug.Log("ÉQÅ[ÉÄèIóπ");
        Application.Quit();
    }
}
