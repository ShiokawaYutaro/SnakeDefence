using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float moveSpeed = 3f;
    private float viewAngle = 130f;
    private int rayCount = 20;
    private float rayDistance = 4f;
    private float groundCheckDistance = 10f;
    private float turnSpeed = 90f;
    private float turnDuration = 1.0f;

    private float chaseViewDistance = 10f; // ← 見つける距離を伸ばしたい場合ここ
    private float chaseViewAngle = 120f;
    private string playerTag = "Player";

    private float snakeAmplitude = 1f;
    private float snakeFrequency = 3f;
    private float sideCheckDistance = 2.0f;
    private float swayAvoidStrength = 1.0f;

    private float snakeTimer = 0f;

    private Rigidbody rb;
    private GameObject player;

    private bool isTurning = false;
    private float turnTimeRemaining = 0f;
    private float turnDirection = 0f;
    private float lastTurnDirection = 0f;

    private int layerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag(playerTag);
        layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));
    }

    void FixedUpdate()
    {
        snakeTimer += Time.fixedDeltaTime * snakeFrequency;

        if (player != null && CanSeePlayer())
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime));

            Vector3 sway = GetSafeSwayDirection();
            Vector3 moveDirection;

            if (IsFrontClear())
            {
                moveDirection = (transform.forward + sway).normalized;
            }
            else
            {
                // 回避を優先：正面がふさがれていても止まらずに左右に移動
                moveDirection = (sway.magnitude > 0.1f ? sway : -transform.right).normalized;
            }

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            Debug.DrawLine(transform.position, player.transform.position, Color.yellow);
            return;
        }

        if (isTurning)
        {
            Turn();
            return;
        }

        bool frontClear = IsFrontClear();
        bool groundAhead = IsGroundAhead();

        if (groundAhead)
        {
            Vector3 sway = GetSafeSwayDirection();
            Vector3 moveDirection;

            if (frontClear)
            {
                moveDirection = (transform.forward + sway).normalized;
            }
            else
            {
                moveDirection = sway.normalized;
            }

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isTurning = true;
            turnTimeRemaining = turnDuration;

            do
            {
                turnDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
            } while (turnDirection == lastTurnDirection);

            lastTurnDirection = turnDirection;
        }
    }

    void Turn()
    {
        Quaternion turn = Quaternion.Euler(0, turnSpeed * turnDirection * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * turn);
        turnTimeRemaining -= Time.fixedDeltaTime;

        if (turnTimeRemaining <= 0f)
        {
            isTurning = false;
        }
    }

    Vector3 GetSafeSwayDirection()
    {
        float swayAmount = Mathf.Sin(snakeTimer) * snakeAmplitude;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        bool hitRight = Physics.Raycast(origin, transform.right, out RaycastHit hitR, sideCheckDistance, layerMask);
        bool hitLeft = Physics.Raycast(origin, -transform.right, out RaycastHit hitL, sideCheckDistance, layerMask);

        float rightFactor = hitRight ? hitR.distance / sideCheckDistance : 1f;
        float leftFactor = hitLeft ? hitL.distance / sideCheckDistance : 1f;

        // 狭い通路の検知
        float minFactor = Mathf.Min(rightFactor, leftFactor);
        if (minFactor < 0.4f)  // 両側がかなり近い → 揺れを止める
            return Vector3.zero;

        // 壁に近い側のスネークを弱める
        if (swayAmount > 0 && hitRight)
            swayAmount *= rightFactor;
        if (swayAmount < 0 && hitLeft)
            swayAmount *= leftFactor;

        return transform.right * swayAmount;
    }





    bool IsFrontClear()
    {
        float halfAngle = viewAngle / 2f;
        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = Mathf.Lerp(-halfAngle, halfAngle, i / (float)(rayCount - 1));
            Quaternion rotation = Quaternion.Euler(0, angleOffset, 0) * transform.rotation;
            Vector3 direction = rotation * Vector3.forward;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                if (Mathf.Abs(angleOffset) < 5f)
                    return false;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
            }
        }
        return true;
    }

    bool IsGroundAhead()
    {
        Vector3 frontCheckPoint = transform.position + transform.forward * 1f;
        Vector3 direction = Vector3.down;

        bool ground = Physics.Raycast(frontCheckPoint, direction, out RaycastHit hit, groundCheckDistance, layerMask);
        Debug.DrawRay(frontCheckPoint, direction * (ground ? hit.distance : groundCheckDistance), ground ? Color.blue : Color.magenta);

        return ground;
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        float distance = toPlayer.magnitude;
        if (distance > chaseViewDistance) return false;

        Vector3 direction = toPlayer.normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > chaseViewAngle / 2f) return false;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, chaseViewDistance, layerMask))
        {
            if (hit.collider.gameObject == player)
            {
                return true;
            }
        }

        return false;
    }
}
