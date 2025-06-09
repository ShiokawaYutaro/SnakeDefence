using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // アイテム名
    public Sprite icon; // アイコン
    public bool isStackable; // スタック可能かどうか
    public GameObject prefab; // ドロップ時のプレハブ
}