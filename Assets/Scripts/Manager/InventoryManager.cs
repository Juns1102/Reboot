using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : Singleton<InventoryManager> {
    public List<GameObject> items = new List<GameObject>();

    public void SortName()
    {
        QuickSort(items, 0, items.Count - 1, "name");
        Debug.Log("이름 퀵소트");
    }
    public void ReverseSortName()
    {
        QuickSort(items, 0, items.Count - 1, "name");
        ReverseList(items);
        Debug.Log("이름 역순 퀵소트");
    }
    public void SortValue()
    {
        QuickSort(items, 0, items.Count - 1, "value");
        Debug.Log("가치 퀵소트");
    }
    public void ReverseSortValue()
    {
        QuickSort(items, 0, items.Count - 1, "value");
        ReverseList(items);
        Debug.Log("가치 역순 퀵소트");
    }
    public void SortWeight()
    {
        QuickSort(items, 0, items.Count - 1, "weight");
        Debug.Log("무게 퀵소트");
    }
    public void ReverseSortWeight()
    {
        QuickSort(items, 0, items.Count - 1, "weight");
        ReverseList(items);
        Debug.Log("무게 역순 퀵소트");
    }
    public void ReverseList(List<GameObject> list)
    {
        int left = 0;
        int right = list.Count - 1;

        while (left < right) {
            Swap(list, left, right);
            left++;
            right--;
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha5)) ReverseSortName();
        if (Input.GetKeyDown(KeyCode.Alpha6)) ReverseSortValue();
        if (Input.GetKeyDown(KeyCode.Alpha7)) ReverseSortWeight();
    }
}