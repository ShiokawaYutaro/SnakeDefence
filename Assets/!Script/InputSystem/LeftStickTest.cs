using UnityEngine;
using UnityEngine.InputSystem;

public class LeftStickTest : MonoBehaviour
{
    Rigidbody rb;
    Player player;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        // 現在のゲームパッド情報
        var current = Gamepad.current;

        // ゲームパッド接続チェック
        if (current == null)
            return;

        // 左スティック入力取得
        var leftStickValue = current.leftStick.ReadValue();
         Debug.Log($"移動量：{leftStickValue}");
        float moveX = leftStickValue.x;
        float moveZ = leftStickValue.y;

        // 移動
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * player.speed;

        // 回転（移動方向があるときのみ）
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }
}