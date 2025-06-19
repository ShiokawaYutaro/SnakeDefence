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
        // ���݂̃Q�[���p�b�h���
        var current = Gamepad.current;

        // �Q�[���p�b�h�ڑ��`�F�b�N
        if (current == null)
            return;

        // ���X�e�B�b�N���͎擾
        var leftStickValue = current.leftStick.ReadValue();
         Debug.Log($"�ړ��ʁF{leftStickValue}");
        float moveX = leftStickValue.x;
        float moveZ = leftStickValue.y;

        // �ړ�
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * player.speed;

        // ��]�i�ړ�����������Ƃ��̂݁j
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }
}