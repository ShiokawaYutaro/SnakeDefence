using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip StartClip;    
    public AudioClip LobbyClip;    
    public AudioClip StageClip;   
    public AudioClip BossClip;   
    private AudioSource audioSource;

    private bool BossBGMPlay;

    void Start()
    {
        BossStage.startAction = false;
        audioSource = gameObject.AddComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "Start") { audioSource.clip = StartClip; }
        if (SceneManager.GetActiveScene().name == "Lobby") { audioSource.clip = LobbyClip; }
        if (SceneManager.GetActiveScene().name == "Stage") { audioSource.clip = StageClip; }

        audioSource.loop = true;      // ループ再生
        audioSource.volume = 0.3f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }

    private void Update()
    {
        if (BossStage.startAction && !BossBGMPlay)
        {            
            BossBGMPlay = true;
            audioSource.clip = BossClip;
            audioSource.loop = true;      // ループ再生
            audioSource.volume = 0.1f;    // 音量(0〜1)
            audioSource.Play();           // 再生開始
        }
    }
}
