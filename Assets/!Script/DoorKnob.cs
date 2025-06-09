using UnityEngine;

public class DoorKnob : MonoBehaviour
{
    public GameObject PickImage;  // �h�A�m�u��I���ł���A�C�R���iUI�p�j
    private bool isLookingAtDoor = false;  // �h�A�m�u�Ɏ��������킹�Ă��邩�ǂ���
    private bool isDoorOpen = false;  // �h�A���J���Ă��邩�ǂ���

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // �}�E�X�̈ʒu���烌�C���΂�

        // �h�A�m�u�ɃJ�[�\�������킹�Ă��邩�̊m�F
        if (Physics.Raycast(ray, out hit, 3f))  // 3f �͋����i�����j
        {
            if (hit.collider.CompareTag("ClassroomDoornob"))
            {
                isLookingAtDoor = true;
                PickImage.SetActive(true);  // �h�A�m�uUI��\��
            }
            else
            {
                isLookingAtDoor = false;
                PickImage.SetActive(false);  // �h�A�m�uUI���\��
            }
        }
        else
        {
            PickImage.SetActive(false);  // ���E�O�Ȃ��\��
        }

        // �h�A�m�u�𑀍삷�鏈��
        if (isLookingAtDoor && Input.GetKeyDown(KeyCode.E))  // E�L�[�ő���
        {
            if (isDoorOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    // �h�A���J���鏈��
    private void OpenDoor()
    {
        isDoorOpen = true;
        Debug.Log("�h�A���J���܂���");

        // �h�A���J����A�j���[�V�����⃍�W�b�N�������ɒǉ��ł��܂�
    }

    // �h�A��߂鏈��
    private void CloseDoor()
    {
        isDoorOpen = false;
        Debug.Log("�h�A��߂܂���");

        // �h�A��߂�A�j���[�V�����⃍�W�b�N�������ɒǉ��ł��܂�
    }
}
