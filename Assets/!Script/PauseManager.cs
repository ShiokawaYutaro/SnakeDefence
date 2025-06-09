using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //[SerializeField]
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject SystemLayer;


    enum PauseType
    {
        Normal,
        HomePause,
    }

    PauseType pauseType;

    bool OnKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (pauseType)
        {
            case PauseType.Normal:
                {
                    Normal();
                    break;
                }

            case PauseType.HomePause: 
                {
                    HomePause();
                    break;
                }
        }

        if (Input.GetKeyUp(KeyCode.Escape) && OnKey)
        {
            OnKey = false;
        }
    }

    void Normal()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !OnKey)
            {
                pauseType = PauseType.HomePause;
                OnKey = true;
            }
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            PauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void HomePause()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !OnKey)
            {
                pauseType = PauseType.Normal;
                OnKey = true;
            }
            //Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            PauseScreen.SetActive(true);
            Time.timeScale = 0;
        }


    }
}
