using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClick()
    {
        animator.SetTrigger("start");
    }

    public void ExitClick()
    {
        animator.SetTrigger("exit");
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void StartSE()
    {
        SEManager.Instance.StartSE();
    }

    public void ExitGame()
    {
        Debug.Log("ÉQÅ[ÉÄèIóπ");
        Application.Quit();
    }
}
