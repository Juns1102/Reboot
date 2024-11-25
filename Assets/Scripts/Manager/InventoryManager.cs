using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : Singleton<InventoryManager> {
    [SerializeField]
    public int value;
    public int capacity;
    public int currentCapacity;
    public List<ItemData> items = new List<ItemData>();

    private void Awake() {
        currentCapacity = 0;
    }

    private void SortName() {

    }

    private void SortValue() {

    }

    private void SortWeight() {

    }

    public void GetHeadInfo(TextMeshProUGUI value, TextMeshProUGUI capacity) {
        value.text = this.value.ToString();
        capacity.text = currentCapacity + "/" + this.capacity;
        capacity.color = currentCapacity > this.capacity ? Color.red : Color.white;
    }

    public bool CheckCapacity() {
        return currentCapacity <= capacity;
    }

    public void RemoveItem(int num) {
        if(num != -1) {
            value -= items[num].value;
            currentCapacity -= items[num].weight;
            items.RemoveAt(num);
        }
    }
}
