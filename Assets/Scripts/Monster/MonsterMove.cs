using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    public Vector2 playerPos; //플레이어 좌표
    public Vector2 myPos; //몬스터 좌표
    public int[,] weight; //가중치 배열
    public List<Vector2> vectorList;
    public Vector2 vector;

    //먼저 주변 맵 정보를 수집
    //수집한 정보를 토대로 길 찾기
    //최소 경로 추적

    //플레이어, 몬스터 사이 범위 맵 정보 수집
    void GetMapInfo() {
		myPos = this.transform.position;
        int xDistance = (int)(myPos.x - playerPos.x); //몬스터 - 플레이어 x거리
        int yDistance = (int)(myPos.y - playerPos.y); //몬스터 - 플레이어 y거리
		Debug.Log(xDistance + ", " + yDistance);
		weight = new int[Mathf.Abs(yDistance) + 1, Mathf.Abs(xDistance) + 1]; //몽총하게 크기 -1로 받아버림.........
        RaycastHit2D hit; //레이어 확인 변수

        if(yDistance == 0) { //x축
			for (int x = 0; x < Mathf.Abs(xDistance); x++) {
				hit = Physics2D.Raycast(myPos + new Vector2(xDistance > 0 ? -x : x, 0), new Vector2(0, 0), LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
				if (hit.collider == null) { //평지(가중치 1)일 경우
					Debug.Log("Layer: " + hit.collider + " (x, y): (" + yDistance + ", " + xDistance + ")");
					weight[yDistance, x] = 1;
				}
				else { //장애물(가중치 INF) 존재할 경우
					Debug.Log("Layer: " + hit.collider + " (x, y): (" + yDistance + ", " + xDistance + ")");
					weight[yDistance, x] = INF;
				}
			}
		}
        else if(xDistance == 0) { //y축	
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				hit = Physics2D.Raycast(myPos + new Vector2(0, yDistance > 0 ? -y : y), myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
				if (hit.collider == null) { //평지(가중치 1)일 경우
					weight[y, 0] = 1;
				}
				else { //장애물(가중치 INF) 존재할 경우
					weight[y, 0] = INF;
				}
			}
		}
        else if(xDistance < 0 && yDistance < 0) { //제 1사분면
            for (int y = 0; y < Mathf.Abs(yDistance); y++) {
                for(int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, y), myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
                        weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
                        weight[y, x] = INF;
					}
				}
            }
        }
        else if(xDistance > 0 && yDistance < 0) { //제 2사분면
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, y), myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
                        weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
                        weight[y, x] = INF;
					}
				}
			}
		}
        else if(xDistance > 0 && yDistance > 0) { //제 3사분면
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, -y), myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
                        weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
                        weight[y, x] = INF;
					}
				}
			}
		}
        else if(xDistance < 0 && yDistance > 0) { //제 4사분면
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, -y), myPos, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
						weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
						weight[y, x] = INF;
					}
				}
			}
		}
    }

    int Dfs(int x, int y, int distance, ref int minWeight) {
        if(distance < minWeight) {
			if(x < weight.GetLength(y) - 1) { //수평 이동
                vectorList.Add(new Vector2(1, 0));
				distance = Dfs(x + 1, y, distance + weight[y, x], ref minWeight);
            }
            if(y < weight.Length - 1) { //수직 이동
                vectorList.Add(new Vector2(0, 1));
				distance = Dfs(x, y + 1, distance + weight[y, x], ref minWeight);
            }

            if(distance < minWeight) {
                minWeight = distance;
                vector = vectorList[0];
            }
            vectorList.RemoveAt(vectorList.Count - 1);
		}

        return minWeight;
    }

    Vector2 Chasing() {
		int minWeight = INF;
        int distance;

		//초기화
		GetMapInfo();

		Debug.Log(weight);

        distance = Dfs(0, 0, 0, ref minWeight);
        Debug.Log("Shortest: " + distance); //최단 거리 확인용 로그 출력

        return vector;
    }

	void Wandering() {
    }

    public void Move() {
		playerPos = GameObject.Find("TestPlayer").transform.position;
		myPos = this.transform.position;
        if(Vector2.Distance(myPos, playerPos) <= 5) { //플레이어 - 몬스터 거리 5 이하면
            Chasing(); //플레이어 추격
        }
        else {
            Wandering(); //랜덤 이동
        }
    }
}
