using UnityEngine;
using UnityEngine.InputSystem;

public class LeftStickTest : MonoBehaviour
{
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
    }
}