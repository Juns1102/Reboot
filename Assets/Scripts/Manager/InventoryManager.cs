using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : Singleton<InventoryManager> {
    public List<GameObject> items = new List<GameObject>();

    public void SortName()
    {
        QuickSort(items, 0, items.Count - 1, "name");
        Debug.Log("ÀÌ¸§ Äü¼ÒÆ®");
    }

    public void SortValue()
    {
        QuickSort(items, 0, items.Count - 1, "value");
        Debug.Log("°¡Ä¡ Äü¼ÒÆ®");
    }

    public void SortWeight()
    {
        QuickSort(items, 0, items.Count - 1, "weight");
        Debug.Log("¹«°Ô Äü¼ÒÆ®");
    }

    private void QuickSort(List<GameObject> list, int left, int right, string Type)
    {
        if (left < right) {
            int q = Partition(list, left, right, Type);
            QuickSort(list, left, q - 1, Type);
            QuickSort(list, q + 1, right, Type);
        }
    }

    private int Partition(List<GameObject> list, int left, int right, string Type)
    {
        GameObject pivot = list[left];
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


    private int Compare(GameObject a, GameObject b, string Type)
    {
        Item item1 = a.GetComponent<Item>();
        Item item2 = b.GetComponent<Item>();

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

    private void Swap(List<GameObject> list, int a, int b)
    {
        GameObject temp = list[a];
        list[a] = list[b];
        list[b] = temp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SortName();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SortValue();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SortWeight();
    }
}