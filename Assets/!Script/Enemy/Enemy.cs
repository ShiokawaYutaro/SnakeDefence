using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Enemy : Character
{
    private GameObject player;
    GameObject target;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;
    bool atkDelay;

    float viewAngle = 120f;
    int rayCount = 20;
    float rayDistance = 1f;

    protected override void Start()
    {
        target = GameObject.Find("家");
        base.Start();
        MaxHp = 100f;
        speed = 3;
    }

    protected override void FixedUpdate()
    {
        healthImage.transform.LookAt(Camera.main.transform.position);
        
       // rb.velocity = transform.forward * speed;

        if(rb.velocity.magnitude >= 0.01f)
        {
            animator.SetBool("attack", false);
            animator.SetBool("run", true);
        }

        ViewAction();
    }

    void ViewAction()
    {
        Vector3 viewPos = new Vector3(transform.position.x,transform.position.y + 1,transform.position.z);
        Ray ray = new Ray(viewPos, transform.forward);

        float halfAngle = viewAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            // -halfAngle ～ +halfAngle の範囲で角度を振る
            float t = (float)i / (rayCount - 1); // 0〜1
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            // 自分の forward をベースに角度を加える（Y軸回転）
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 dir = rotation * transform.forward;
            if (Physics.Raycast(viewPos,dir , out RaycastHit hit, rayDistance))
            {
                if (hit.collider.tag == "Player")
                {
                    rb.velocity = Vector3.zero;
                    animator.SetBool("run", false);
                    animator.SetBool("attack", true);
                    transform.LookAt(hit.collider.transform.position);
                }

                //else if (hit.collider.transform.parent.name == "家")
                //{
                //    rb.velocity = Vector3.zero;
                //    animator.SetBool("run", false);
                //    animator.SetBool("attack", true);
                //    transform.LookAt(target.transform.position);
                //}


                else
                {
                    rb.velocity = transform.forward * speed;
                    transform.LookAt(target.transform.position);
                }
            }
            else
            {
                rb.velocity = transform.forward * speed;
                transform.LookAt(target.transform.position);
            }
        }
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
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

        GameObject damageText = Instantiate(damageNotation,transform.Find("UI/healthImage"));
        damageText.GetComponent<Text>().text = _damage.ToString();
        Destroy(damageText , 1);
    }
}
