using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField]
    private bool playerTurn;
    
    public int playerHearts;
    public List<Item> fieldItems = new List<Item>();
    public int limitWeight;  

    private void Start()
    {
        playerHearts = 3;
        (int maxValue, List<Item> selectedItems) = SolveKnapsack(fieldItems, limitWeight);
        
        // Debug.Log("선택된 물건");
        // foreach (var item in selectedItems) {
        //     Debug.Log($"물건: {item.data.name}, 가치: {item.data.value}, 무게: {item.data.weight}");
        // }
        // Debug.Log($"이 맵에서 얻을 수 있는 최대 가치: {maxValue}");
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
            int wi = items[i - 1].data.weight;
            int vi = items[i - 1].data.value;

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
                remain -= items[i - 1].data.weight;
            }
        }

        return (K[n, limitWeight], selectedItems);
    }

    public bool turnCheck() {
        return playerTurn;
    }

    public void turnChange() {
        playerTurn = !playerTurn;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            (int maxValue, List<Item> selectedItems) = SolveKnapsack(fieldItems, limitWeight);

            Debug.Log("선택된 물건");
            foreach (var item in selectedItems) {
                Debug.Log($"물건: {item.name}, 가치: {item.data.value}, 무게: {item.data.weight}");
            }
            Debug.Log($"이 맵에서 얻을 수 있는 최대 가치: {maxValue}");
        }
    }
}
