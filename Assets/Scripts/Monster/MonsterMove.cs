using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    Vector2 playerPos; //플레이어 좌표
    Vector2 myPos; //몬스터 좌표
    RaycastHit2D hit;
    Vector2[][] path;
    int[][] weight;

    //먼저 주변 맵 정보를 수집
    //수집한 정보를 토대로 길 찾기
    //최소 경로 추적

    void GetMapInfo() {
        RaycastHit2D hit = Physics2D.Raycast(myPos, myPos, LayerMask.GetMask("Platform"));
        if(myPos.x <= playerPos.x) {
            if(myPos.y <= playerPos.y) {
                for(int i = 0; i < playerPos.x - myPos.x; i++) {
                    //if(Physics2D.Raycast(myPos + new Vector2(i, 0), 1))
                }
            }
        }
        else {

        }
    }

	void Chasing() {
        int i, u, w;

        for(i = 0; i < Mathf.Abs(myPos.x - playerPos.x) + Mathf.Abs(myPos.y - playerPos.y); i++) {

        }

        //간선 개수 == (몬스터x  - 플레이어x) + (몬스터y - 플레이어y)
        for(i = 0; i < Mathf.Abs(myPos.x - playerPos.x) + Mathf.Abs(myPos.y - playerPos.y); i++) {
            
        }
	}

	/*
    void shortest_path(GraphType* g, int start) {
        int i, u, w;
        for (i = 0; i<g->n; i++) { 초기화
            distance[i] = g->weight[start][i];
            found[i] = FALSE;
        }
        found[start] = TRUE;    시작 정점 방문 표시
        distance[start] = 0;
        for (i = 0; i < g->n - 1; i++) {
	        print_status(g);
	        u = choose(distance, g->n, found);
	        found[u] = TRUE;
	        for (w = 0; w < g->n; w++)
		        if (!found[w])
        			if (distance[u] + g->weight[u][w] < distance[w])
				        distance[w] = distance[u] + g->weight[u][w];
        }
    }
    */

	void Wandering() {
        /*
        switch(Random.Range(0, 4)) {
            case 0: //상
				hit = Physics2D.Raycast(myPos, transform.forward, 1, LayerMask.GetMask("Platform"));
				break;
            case 1: //하
				hit = Physics2D.Raycast(myPos, -transform.forward, 1, LayerMask.GetMask("Platform"));
				break;
            case 2: //좌
				hit = Physics2D.Raycast(myPos, transform.right, 1, LayerMask.GetMask("Platform"));
				break;
            case 3: //우
				hit = Physics2D.Raycast(myPos, -transform.right, 1, LayerMask.GetMask("Platform"));
				break;
        }
        */
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

    // Update is called once per frame
    void Update() {
		RaycastHit2D hit = Physics2D.Raycast(myPos, new Vector2(0, 0), LayerMask.GetMask("Platform"));
        Debug.Log(hit.collider);
	}
}
