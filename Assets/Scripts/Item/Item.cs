using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    public ItemData data;

    private void SetUp(ItemData data) {
        gameObject.name = data.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    private void Awake() {
        SetUp(data);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (InventoryManager.Instance.CheckCapacity()) {
                InventoryManager.Instance.items.Add(data);
                gameObject.SetActive(false);
                UIManager.Instance.ItemPlace();
                InventoryManager.Instance.currentCapacity += data.weight;
                InventoryManager.Instance.value += data.value;
                UIManager.Instance.SetHeadInfo();
            }
        }
    }
}
