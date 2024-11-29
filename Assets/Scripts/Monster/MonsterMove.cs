using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    Vector2 playerPos; //플레이어 좌표
    Vector2 myPos; //몬스터 좌표
    int[][] weight; //가중치 배열
    

    //먼저 주변 맵 정보를 수집
    //수집한 정보를 토대로 길 찾기
    //최소 경로 추적

    //플레이어, 몬스터 사이 범위 맵 정보 수집
    void GetMapInfo() {
        Array.Clear(weight, 0, weight.Length); //가중치 배열 초기화(움직일 때마다 범위, 값 바껴서 초기화 해줘야함)
        float xDistance = Mathf.Abs(myPos.x - playerPos.x); //플레이어 - 몬스터 x거리
        float yDistance = Mathf.Abs(myPos.y - playerPos.y); //플레이어 - 몬스터 y거리
        RaycastHit2D hit; //레이어 확인 변수

        for(int i = 0; i < xDistance; i++) {
            for(int j = 0; j < yDistance; j++) {
                hit = Physics2D.Raycast(myPos, myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
                if(hit.collider == null) { //평지(가중치 1)일 경우
                    weight[i][j] = 1;
                }
                else { //장애물(가중치 INF) 존재할 경우
                    weight[i][j] = INF;
                }
            }
        }
    }

    int MoveHorizontal(int x, int minWeight) {
        //가중치 합 < 최소라면 ㄱㄱ
        if(x < playerPos.x) {
            MoveHorizontal(x + 1, minWeight);
        }
        else if(x > playerPos.x) {
            MoveHorizontal(x - 1, minWeight);
        }

        return weight[x][x]; //임시 반환
    }

    void MoveVertical(int y, int minWeight) {
        if(y < playerPos.y) {
        
        }
        else if(y > playerPos.y) {

        }
        else {
            return;
        }
    }

    void Dfs() {
        MoveHorizontal((int)myPos.x, INF);
        MoveVertical((int)myPos.y, INF);
    }

    Vector2 Chasing() {
        //초기화
        GetMapInfo();

        Dfs();

        return myPos; //임시 반환
    }

	void Wandering() {
    }

    void Move() {
        myPos = this.transform.position;
        if(Vector2.Distance(myPos, playerPos) <= 5) { //플레이어 - 몬스터 거리 5 이하면
            Chasing(); //플레이어 추격
        }
        else {
            Wandering(); //랜덤 이동
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        playerPos = GameObject.Find("TestPlayer").transform.position;
        myPos = this.transform.position;
    }
}
