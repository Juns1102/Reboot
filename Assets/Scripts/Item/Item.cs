using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    //public int value;
    //public int weight;

    private void SetUp(ItemData data) {
        gameObject.name = data.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
        //this.value = data.value;
        //this.weight = data.weight;
    }

    private void Awake() {
        SetUp(data);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            InventoryManager.Instance.items.Add(data);
            gameObject.SetActive(false);
            UIManager.Instance.ItemPlace();
        }
    }
}
