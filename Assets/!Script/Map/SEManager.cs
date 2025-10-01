using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;
    public AudioClip AttackClip;    
    public AudioClip DamageClip;    
    public AudioClip ULTAttackClip;    
    private AudioSource audioSource;

    public AudioClip CanBuyPerk;
    public AudioClip NoCanBuyPerk;

    public AudioClip StartClip;
    public AudioClip LVLUPClip;
    void Start()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlayerAttackSE()
    {
        audioSource.clip = AttackClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.3f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }

    public void EnemyAttackSE()
    {
        audioSource.clip = DamageClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.3f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }

    public void PlayerULTAttackSE()
    {
        audioSource.clip = ULTAttackClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.3f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }

    public void CanBuySE()
    {
        audioSource.clip = CanBuyPerk;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 1f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }
    public void NoCanBuySE()
    {
        audioSource.clip = NoCanBuyPerk;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 1f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }
    public void StartSE()
    {
        audioSource.clip = StartClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 1f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }
    public void LVLUPSE()
    {
        audioSource.clip = LVLUPClip;
        audioSource.loop = false;      // ループ再生
        audioSource.volume = 0.3f;    // 音量(0〜1)
        audioSource.Play();           // 再生開始
    }
}
