using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Singleton
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
    #endregion
    
    public bool ableTP;
    public bool inTp;
    public float skill1MaxCoolTime;
    public float skill2MaxCoolTime;
    public float skill1CoolTime;
    public float skill2CoolTime;
    public bool equipTp;
    public bool activeSkill;
    public bool playerTurn;
    public bool enemyZone;
    public string planetName;
    public int playerHearts;
    public List<Item> fieldItems = new List<Item>();
    public List<Item> selectedItems = new List<Item>();
    public List<GameObject> enemies = new List<GameObject>();
    public int limitWeight;
    public int maxValue;
    public delegate void Teleport();
    public Teleport teleport;
    public int moveCount;
    public int maxMoveCount;
    public int map1Value;
    public int map2Value;
    public int map3Value;

    private void Start() {
        playerHearts = 3;
        SolveKnapsack(fieldItems, limitWeight);
    }

    public void CheckEnemies(){
        int n=0;
        for(int i=0; i<enemies.Count; i++){
            if(Vector2.Distance(enemies[i].transform.position, GameObject.Find("TestPlayer").transform.position) <= 5){
                n++;
            }
        }
        if(n > 0){
            enemyZone = true;
        }
        else{
            enemyZone = false;
        }
    }
    public void EnemiesMove(){
        for(int i=0; i < enemies.Count; i++){
            enemies[i].GetComponent<MonsterMove>().Move();
        }
    }
    public void Skill1Set(){
        activeSkill = false;
        skill1CoolTime = skill1MaxCoolTime;
    }

    public void Skill2Set(){
        activeSkill = false;
        skill2CoolTime = skill2MaxCoolTime;
    }

    public void SkillCoolDown(){
        if(skill1CoolTime > 0){
            skill1CoolTime--;
        }
        if(skill2CoolTime > 0){
            skill2CoolTime--;
        }
    }

    public void SetTp(bool moveStop){
        this.ableTP = moveStop;
    }

    public void SetTp(int tpNum){
        inTp = tpNum != 0 ? true : false;
    }

    public void SetMaxValueInfo(){
        SolveKnapsack(fieldItems, limitWeight);
    }

    public void SelectTp(int num=0){
        if(num == 0){
            teleport = Teleport0;
        }
        else if(num == 1){
            teleport = Teleport1;
        }
        else if(num == 2){
            teleport = Teleport2;
        }
        else if(num == 3){
            teleport = Teleport3;
        }
    }

    public void Teleport0(){
        if(ableTP && inTp){
            SceneChanger.Instance.ChangeMap1();
            planetName = "Lobby";
        }
    }
    public void Teleport1(){
        if(ableTP && inTp){
            SceneChanger.Instance.ChangeMap2();
            planetName = "Ancient Ruins";
        }
    }
    public void Teleport2(){
        if(ableTP && inTp){
            SceneChanger.Instance.ChangeMap3();
            planetName = "Atlantis";
        }
    }
    public void Teleport3(){
        if(ableTP && inTp){
            SceneChanger.Instance.ChangeMap4();
            planetName = "GraveYard";
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
}
