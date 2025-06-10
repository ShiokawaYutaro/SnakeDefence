using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : Character
{
    private float moveSpeed = 3f;
    private float viewAngle = 130f;
    private int rayCount = 20;
    private float rayDistance = 4f;
    private float groundCheckDistance = 10f;
    private float turnSpeed = 90f;
    private float turnDuration = 1.0f;

    private GameObject player;

    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;



    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
        MaxHp = 10f;
        speed = 10;
    }

    protected override void FixedUpdate()
    {
      
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Snake"))
        {
            SetDamage(1);
        }
        
    }

    private void UpdateFillAmount(Image image, ref float currentRate, float targetRate, float duration)
    {
        // 0〜1の範囲に制限
        targetRate = Mathf.Clamp01(targetRate);

        // DOTweenでFillAmountのアニメーション
        image.DOFillAmount(targetRate, duration);

        // currentRateの更新
        currentRate = targetRate;
    }

    public void SetDamage(float _damage)
    {
        HP -= _damage;
        float targetRate = HcurrentRate - _damage / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
    }
}
