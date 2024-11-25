using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#pragma warning disable CS0618

public class FractionalKnapsack : MonoBehaviour
{
    public int limitWeight;  
    private List<Item> Items = new List<Item>();

    private void Start()
    {
        Items.AddRange(FindObjectsOfType<Item>());

        (int maxValue, List<Item> selectedItems) = SolveKnapsack(Items, limitWeight);

        Debug.Log("���õ� ����");
        foreach (var item in selectedItems) {
            Debug.Log($"����: {item.name}, ��ġ: {item.value}, ����: {item.weight}");
        }
        Debug.Log($"�� �ʿ��� ���� �� �ִ� �ִ� ��ġ: {maxValue}");
    }

    public (int maxValue, List<Item>) SolveKnapsack(List<Item> items, int limitWeight)
    {
        int n = items.Count;

        int[,] K = new int[n + 1, limitWeight + 1];

        for (int i = 0; i <= n; i++) {
            K[i, 0] = 0;
        }

        for (int w = 0; w <= limitWeight; w++) {
            K[0, w] = 0;
        }

        for (int i = 1; i <= n; i++) {
            int wi = items[i - 1].weight;
            int vi = items[i - 1].value;

            for (int w = 1; w <= limitWeight; w++) {
                if (wi > w) {
                    K[i, w] = K[i - 1, w];
                }
                else {
                    K[i, w] = Mathf.Max(
                        K[i - 1, w],
                        K[i - 1, w - wi] + vi
                    );
                }
            }
        }

        List<Item> selectedItems = new List<Item>();
        int remain = limitWeight;

        for (int i = n; i > 0 && remain > 0; i--) {
            if (K[i, remain] != K[i - 1, remain]) {
                selectedItems.Add(items[i - 1]);
                remain -= items[i - 1].weight;
            }
        }

        return (K[n, limitWeight], selectedItems);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            (int maxValue, List<Item> selectedItems) = SolveKnapsack(Items, limitWeight);

            Debug.Log("���õ� ����");
            foreach (var item in selectedItems) {
                Debug.Log($"����: {item.name}, ��ġ: {item.value}, ����: {item.weight}");
            }
            Debug.Log($"�� �ʿ��� ���� �� �ִ� �ִ� ��ġ: {maxValue}");
        }
    }
}