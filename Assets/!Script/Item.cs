using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // �A�C�e����
    public Sprite icon; // �A�C�R��
    public bool isStackable; // �X�^�b�N�\���ǂ���
    public GameObject prefab; // �h���b�v���̃v���n�u
}