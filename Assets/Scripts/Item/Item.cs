using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int value;
    public int weight;

    private void SetUp(ItemData data) {
        gameObject.name = data.name;
        this.value = data.value;
        this.weight = data.weight;
    }

    private void Awake() {
        SetUp(data);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            InventoryManager.Instance.items.Add(gameObject);
            gameObject.SetActive(false);
        }
    }
}
