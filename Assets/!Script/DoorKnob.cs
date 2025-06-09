using UnityEngine;

public class DoorKnob : MonoBehaviour
{
    public GameObject PickImage;  // ドアノブを選択できるアイコン（UI用）
    private bool isLookingAtDoor = false;  // ドアノブに視線を合わせているかどうか
    private bool isDoorOpen = false;  // ドアが開いているかどうか

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // マウスの位置からレイを飛ばす

        // ドアノブにカーソルを合わせているかの確認
        if (Physics.Raycast(ray, out hit, 3f))  // 3f は距離（調整可）
        {
            if (hit.collider.CompareTag("ClassroomDoornob"))
            {
                isLookingAtDoor = true;
                PickImage.SetActive(true);  // ドアノブUIを表示
            }
            else
            {
                isLookingAtDoor = false;
                PickImage.SetActive(false);  // ドアノブUIを非表示
            }
        }
        else
        {
            PickImage.SetActive(false);  // 視界外なら非表示
        }

        // ドアノブを操作する処理
        if (isLookingAtDoor && Input.GetKeyDown(KeyCode.E))  // Eキーで操作
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

    // ドアを開ける処理
    private void OpenDoor()
    {
        isDoorOpen = true;
        Debug.Log("ドアを開けました");

        // ドアを開けるアニメーションやロジックをここに追加できます
    }

    // ドアを閉める処理
    private void CloseDoor()
    {
        isDoorOpen = false;
        Debug.Log("ドアを閉めました");

        // ドアを閉めるアニメーションやロジックをここに追加できます
    }
}
