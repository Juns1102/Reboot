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

    public void SortName()
    {
        QuickSort(items, 0, items.Count - 1, "name");
        UIManager.Instance.ItemPlace();
        UIManager.Instance.BottumInfoReset();
    }

    public void SortValue()
    {
        QuickSort(items, 0, items.Count - 1, "value");
        UIManager.Instance.ItemPlace();
        UIManager.Instance.BottumInfoReset();
    }

    public void SortWeight()
    {
        QuickSort(items, 0, items.Count - 1, "weight");
        UIManager.Instance.ItemPlace();
        UIManager.Instance.BottumInfoReset();
    }

    private void QuickSort(List<ItemData> list, int left, int right, string Type)
    {
        if (left < right) {
            int q = Partition(list, left, right, Type);
            QuickSort(list, left, q - 1, Type);
            QuickSort(list, q + 1, right, Type);
        }
    }

    private int Partition(List<ItemData> list, int left, int right, string Type)
    {
        ItemData pivot = list[left];
        int low = left;
        int high = right + 1;

        do {
            do {
                low++;
            } while (low <= right && Compare(list[low], pivot, Type) < 0);

            do {
                high--;
            } while (high >= left && Compare(list[high], pivot, Type) > 0);

            if (low < high) {
                Swap(list, low, high);
            }
        } while (low < high);

        Swap(list, left, high); 
        return high;
    }


    private int Compare(ItemData a, ItemData b, string Type)
    {
        ItemData item1 = a;
        ItemData item2 = b;

        switch (Type) {
            case "name":
                return item1.name.CompareTo(item2.name);
            case "value":
                return item1.value.CompareTo(item2.value); 
            case "weight":
                return item1.weight.CompareTo(item2.weight); 
            default:
                return 0;
        }
    }

    private void Swap(List<ItemData> list, int a, int b)
    {
        ItemData temp = list[a];
        list[a] = list[b];
        list[b] = temp;
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
