using UnityEngine;

public class ItemPick : MonoBehaviour
{
    public Item item;  // 拾うアイテムのデータ
    public float pickupRange = 3f;  // 拾える範囲
    [SerializeField] Camera mainCamera;

    private bool pickedUp = false;

    public Renderer itemRenderer;
    public Material highlightMaterial;
    private Material originalMaterial;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("Player").transform
                .Find("Armature/尻/背骨/胴体/首/頭/Camera").GetComponent<Camera>();
        }

        if (itemRenderer != null)
        {
            originalMaterial = itemRenderer.material;
        }
    }

    private void Update()
    {
        if (pickedUp) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            ItemPick itemToPickUp = hit.collider.GetComponent<ItemPick>();

            if (itemToPickUp == this)
            {
                // ハイライトする
                if (itemRenderer != null && highlightMaterial != null)
                    itemRenderer.material = highlightMaterial;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickedUp = true;

                    Inventory playerInventory = FindObjectOfType<Inventory>();
                    playerInventory.AddItem(itemToPickUp.item);

                    InventoryUI ui = FindObjectOfType<InventoryUI>();
                    if (ui != null)
                        ui.DrawUI();

                    Debug.Log("Item picked up: " + itemToPickUp.item.itemName);

                    Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
                }
            }
            else
            {
                // 元のマテリアルに戻す
                if (itemRenderer != null && originalMaterial != null)
                    itemRenderer.material = originalMaterial;
            }
        }
        else
        {
            // 見てない場合は元に戻す
            if (itemRenderer != null && originalMaterial != null)
                itemRenderer.material = originalMaterial;
        }
    }
}
