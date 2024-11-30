using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil;

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

    //수평 이동(x좌표, y좌표, 가중치 합, 최소 가중치)
    int MoveHorizontal(int x, int y, int distance, int minWeight) {
        //가중치 합 < 최소라면 ㄱㄱ
        if(distance < minWeight) {
            if(myPos.x < playerPos.x) {
                MoveHorizontal(x + 1, y, distance + weight[y][x], minWeight);
            }
            else if(myPos.x > playerPos.x) {
                MoveHorizontal(x - 1, y, distance + weight[y][weight[y].Length - x], minWeight);
            }
            else {
                return distance;
            }
        }

		return minWeight; //가중치 합 >= 최소인 경우
    }

    int MoveVertical(int x, int y, int distance, int minWeight) {
        if(distance < minWeight) {
			if(myPos.y < playerPos.y) {
                MoveVertical(x, y - 1, distance + weight[weight.Length - y][x], minWeight);
            }
            else if(myPos.y > playerPos.y) {
                MoveVertical(x, y + 1, distance + weight[y][x], minWeight);
            }
            else {
                return distance;
            }
		}

        return minWeight;
    }

    int Dfs(int x, int y, int distance, int minWeight) {
        if(distance < minWeight) {
			if (myPos.x < playerPos.x) {
				Dfs(x + 1, y, distance + weight[x][y], minWeight);
			}
            else if(myPos.x > playerPos.x) {
                Dfs(x - 1, y, distance + weight[x][y], minWeight);
            }
            else if(myPos.y < playerPos.y) {

            }

		}
        
        return distance;
    }

    Vector2 Chasing() {
		int minWeight = INF;
		//초기화
		GetMapInfo();

        Dfs((int)myPos.x, (int)myPos.y, 0, minWeight);

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
