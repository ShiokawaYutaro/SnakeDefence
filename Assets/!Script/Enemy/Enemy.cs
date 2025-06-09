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
    }

    void FixedUpdate()
    {

    }
}
