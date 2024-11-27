using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    public static GameManager Instance{
        get{
            if(null == instance){
                return null;
            }
            return instance;
        }
    }

    private void Awake() {
        if(instance == null){
            instance = this;
            if(transform.parent != null && transform.root != null){
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else{
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else{
            Destroy(this.gameObject);
        }
    }
    
    [SerializeField]
    private bool playerTurn;
    private int tpNum;
    private bool ableTP;
    private bool inTp;
    public string planetName;
    public int playerHearts;
    public List<Item> fieldItems = new List<Item>();
    public List<Item> selectedItems = new List<Item>();
    public int limitWeight;  
    public int maxValue;

    private void Start() {
        playerHearts = 3;
        SolveKnapsack(fieldItems, limitWeight);
    }

    public void SetTp(bool moveStop){
        this.ableTP = moveStop;
    }

    public void SetTp(int tpNum){
        this.tpNum = tpNum;
        inTp = tpNum == 0 ? true : false;
    }

    public void Teleport(){
        if(ableTP){
            if(tpNum == 1){
                SceneChanger.Instance.ChangeMap1();
            }
            else if(tpNum == 2){
                SceneChanger.Instance.ChangeMap2();
            }
            else if(tpNum == 3){
                SceneChanger.Instance.ChangeMap3();
            }
            else if(tpNum == 4){
                SceneChanger.Instance.ChangeMap4();
            }
        }
    }

    public void SolveKnapsack(List<Item> items, int limitWeight) {
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

        int remain = limitWeight;

        for (int i = n; i > 0 && remain > 0; i--) {
            if (K[i, remain] != K[i - 1, remain]) {
                selectedItems.Add(items[i - 1]);
                remain -= items[i - 1].data.weight;
            }
        }
        maxValue = K[n, limitWeight];
    }

    public bool turnCheck() {
        return playerTurn;
    }

    public void turnChange() {
        playerTurn = !playerTurn;
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha4)) {
    //         (int maxValue, List<Item> selectedItems) = SolveKnapsack(fieldItems, limitWeight);

    //         Debug.Log("선택된 물건");
    //         foreach (var item in selectedItems) {
    //             Debug.Log($"물건: {item.name}, 가치: {item.data.value}, 무게: {item.data.weight}");
    //         }
    //         Debug.Log($"이 맵에서 얻을 수 있는 최대 가치: {maxValue}");
    //     }
    // }
}
