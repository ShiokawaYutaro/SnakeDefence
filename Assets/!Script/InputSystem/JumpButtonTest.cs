using UnityEngine;
using UnityEngine.InputSystem;

public class JumpButtonTest : MonoBehaviour
{
    private void Update()
    {
        // ���݂̃L�[�{�[�h���
        var current = Keyboard.current;

        // �L�[�{�[�h�ڑ��`�F�b�N
        if (current == null)
            return;

        // �X�y�[�X�L�[�������ꂽ�u�Ԃ��ǂ���
        if (current.spaceKey.wasPressedThisFrame)
            Debug.Log("�X�y�[�X�L�[�������ꂽ�I");
    }
}