using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    private List<GameObject> slotObjects = new List<GameObject>();
    private int selectedIndex = -1;

    [SerializeField] GameObject itemHand;

    void Start()
    {
        DrawUI();
    }

    public void DrawUI()
    {
        // UIの古いスロットを消去
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);
        slotObjects.Clear();

        // インベントリのアイテムをスロットに描画
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            var slot = inventory.slots[i];
            GameObject slotObj = Instantiate(slotPrefab, slotParent);  // スロットのPrefabをインスタンス化
            slotObjects.Add(slotObj);

            // アイコンと数量のUIを設定
            Image icon = slotObj.transform.Find("Icon").GetComponent<Image>();  // アイコンのImageコンポーネントを取得
            Text qty = slotObj.transform.Find("Quantity").GetComponent<Text>();  // 数量のTextコンポーネントを取得
            Button btn = slotObj.GetComponent<Button>();  // スロットのボタンを取得

            if (slot.item != null)
            {
                // アイテムが設定されていれば、アイコンと数量を表示
                icon.sprite = slot.item.icon;  // アイコン画像を設定
                icon.enabled = true;
                qty.text = slot.quantity > 1 ? slot.quantity.ToString() : "";  // 数量が1より多ければ表示
            }
            else
            {
                icon.enabled = false;  // アイテムがない場合はアイコンを非表示
                qty.text = "";
            }

            // ボタンクリック時のイベント設定
            //int index = i;
        }

        HighlightSelectedSlot();  // 選択されたスロットをハイライト
    }

    void HighlightSelectedSlot()
    {
        // すべてのスロットの枠線を無効にし、選択したスロットだけ枠線を表示
        for (int i = 0; i < slotObjects.Count; i++)
        {
            Image border = slotObjects[i].transform.Find("Highlight").GetComponent<Image>();
            if (border != null)
                border.enabled = (i == selectedIndex);
        }

        if (selectedIndex >= 0 && selectedIndex < inventory.slots.Count)
        {
            var slot = inventory.slots[selectedIndex];
            if (slot.item != null)
            {
                if (itemHand.transform.childCount == 1)
                    Destroy(itemHand.transform.GetChild(0).gameObject);

                GameObject prefab = Instantiate(slot.item.prefab, itemHand.transform);
                prefab.transform.Find("obj").GetComponent<Rigidbody>().isKinematic = true;
                prefab.transform.Find("obj").GetComponent<Collider>().enabled = false;
                prefab.transform.localPosition = Vector3.zero;
            }
            
        }
        if (selectedIndex == -1)
        {
            if (itemHand.transform.childCount == 1)
            Destroy(itemHand.transform.GetChild(0).gameObject);
        }



    }

    void Update()
    {
        if (inventory == null || inventory.slots.Count == 0)
            return;

        // 数字キーで直接選択 / 再度押すと解除
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (selectedIndex == i)
                {
                    // 同じスロットをもう一度押したら選択解除
                    selectedIndex = -1;
                }
                else if (i < inventory.slots.Count)
                {
                    selectedIndex = i;
                }

                HighlightSelectedSlot();
            }
        }

        // Qキーでアイテムをドロップ
        if (selectedIndex >= 0 && Input.GetKeyDown(KeyCode.G))
        {
            DropSelectedItem();
        }
    }



    void DropSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.slots.Count)
        {
            var slot = inventory.slots[selectedIndex];
            if (slot.item != null)
            {
                // アイテムをプレイヤーの前にドロップ
                Vector3 dropPos = inventory.transform.position + inventory.transform.forward * 2f;
                Instantiate(slot.item.prefab, dropPos, Quaternion.identity);  // アイテムのプレハブをドロップ

                // インベントリからアイテムを削除
                inventory.RemoveItem(selectedIndex);
                Destroy(itemHand.transform.GetChild(0).gameObject);
                DrawUI();  // UIを更新
            }
        }
    }

}
