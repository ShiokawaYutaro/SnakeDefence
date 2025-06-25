using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;

public class EnemySpawn : Character
{
    [SerializeField] Enemy prefab;
    public float spawnInterval;
    float spawnTime;
    private List<Enemy> enemyList = new List<Enemy> ();


    private Player player;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;
    bool atkDelay;

    public bool dead;

    public bool isDamaged = false;

    protected override void Start()
    {
        MaxHp = 10;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        base.Start();
    }
    protected override void FixedUpdate()
    {
        if (enemyList.Count >= 8) return;

        spawnTime += Time.deltaTime;
        if(spawnTime > spawnInterval)
        {
            spawnTime = 0;
            Enemy enemy = Instantiate(prefab,transform.position,Quaternion.identity);
            enemyList.Add(enemy);
            enemy.enemySpawn = this;
        }

        if (HP <= 0)
        {
            // animator.SetBool("dead", true);
            Dead();
        }

    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
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
        if (dead) return;
        HP -= _damage;
        float targetRate = HcurrentRate - _damage / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
        GameObject damageText = Instantiate(damageNotation, transform.Find("UI/healthImage"));
        damageText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        damageText.GetComponent<Text>().text = _damage.ToString("f1");
        Destroy(damageText, 1);

    }

    CancellationTokenSource cts = new CancellationTokenSource();

    private void OnDestroy()
    {
        cts.Cancel();
    }

    public async void SetAttributeDamage(float _damage, int attribute, Color32 color)
    {
        if (dead) return;
        for (int i = 0; i < attribute; i++)
        {
            if (dead) return;
            HP -= _damage;
            float targetRate = HcurrentRate - _damage / MaxHp;
            UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
            GameObject damageText = Instantiate(damageNotation, transform.Find("UI/healthImage"));
            damageText.GetComponent<Text>().color = color;
            damageText.GetComponent<Text>().text = _damage.ToString("f3");
            Destroy(damageText, 1);

            try
            {
                await UniTask.Delay(2000, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
                // 破棄時にキャンセルされた場合はループを抜ける
                return;
            }
        }
    }


    public void Dead()
    {
        Destroy(gameObject);
        GetComponent<Collider>().enabled = false;
        player.LVLGauge(1);
    }
}
