using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;
    public AudioClip AttackClip;    
    public AudioClip ULTAttackClip;    
    private AudioSource audioSource;

    private bool BossBGMPlay;

    void Start()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlayerAttackSE()
    {
        audioSource.clip = AttackClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.1f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }

    public void PlayerULTAttackSE()
    {
        audioSource.clip = ULTAttackClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.1f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }
}
