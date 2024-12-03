using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil;
using DG.Tweening;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    public Vector2 playerPos; //플레이어 좌표
    public Vector2 myPos; //몬스터 좌표
    public int[,] weight; //가중치 배열
    public List<Vector2> vectorList;
	public Stack<int> distanceStack;
    public Vector2 vector;
    public GameObject pos;
	public Vector2 plusMinus;
    public int coolTime;

    private void Start() {
        GameManager.Instance.enemies.Add(gameObject);
    }


	//먼저 주변 맵 정보를 수집
	//수집한 정보를 토대로 길 찾기
	//최소 경로 추적

	//플레이어, 몬스터 사이 범위 맵 정보 수집
	void GetMapInfo() {
		int xDistance = (int)Mathf.Abs((myPos.x - playerPos.x)) + 1; //몬스터 - 플레이어 x거리
		int yDistance = (int)Mathf.Abs((myPos.y - playerPos.y)) + 1; //몬스터 - 플레이어 y거리
		Vector2 temp;

		weight = new int[yDistance, xDistance];
		RaycastHit2D hit; //레이어 확인 변수

		if (yDistance == 1) { //x축
			for (int x = 0; x < xDistance; x++) {
				temp = myPos + new Vector2((myPos.x - playerPos.x > 0 ? -x : x), 0);
				plusMinus = new Vector2(myPos.x - playerPos.x > 0 ? -1 : 1, 0);
				hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
				if (hit.collider == null) { //평지(가중치 1)일 경우
					weight[0, x] = 1;
				}
				else { //장애물(가중치 INF) 존재할 경우
					weight[0, x] = INF;
				}
			}
		}
		else if (xDistance == 1) { //y축
			for (int y = 0; y < yDistance; y++) {
				temp = myPos + new Vector2(0, myPos.y - playerPos.y > 0 ? -y : y);
				plusMinus = new Vector2(0, myPos.y - playerPos.y > 0 ? -1 : 1);
				hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
				if (hit.collider == null) { //평지(가중치 1)일 경우
					weight[y, 0] = 1;
				}
				else { //장애물(가중치 INF) 존재할 경우
					weight[y, 0] = INF;
				}
			}
		}
		else if (myPos.x - playerPos.x < 0 && myPos.y - playerPos.y < 0) { //제 1사분면
			for (int y = 0; y < yDistance; y++) {
				for (int x = 0; x < xDistance; x++) {
					temp = myPos + new Vector2(x, y);
					plusMinus = new Vector2(1, 1);
					hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
						weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (myPos.x - playerPos.x > 0 && myPos.y - playerPos.y < 0) { //제 2사분면
			for (int y = 0; y < yDistance; y++) {
				for (int x = 0; x < xDistance; x++) {
					temp = myPos + new Vector2(-x, y);
					plusMinus = new Vector2(-1, 1);
					hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
						weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (myPos.x - playerPos.x > 0 && myPos.y - playerPos.y > 0) { //제 3사분면
			for (int y = 0; y < yDistance; y++) {
				for (int x = 0; x < xDistance; x++) {
					temp = myPos + new Vector2(-x, -y);
					plusMinus = new Vector2(-1, -1);
					hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
					if (hit.collider == null) { //평지(가중치 1)일 경우
						weight[y, x] = 1;
					}
					else { //장애물(가중치 INF) 존재할 경우
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (myPos.x - playerPos.x < 0 && myPos.y - playerPos.y > 0) { //제 4사분면
			for (int y = 0; y < yDistance; y++) {
				for (int x = 0; x < xDistance; x++) {
					temp = myPos + new Vector2(x, -y);
					plusMinus = new Vector2(1, -1);
					hit = Physics2D.Raycast(temp, Vector2.right, 0.2f, LayerMask.GetMask("Platform")); //현재 위치 레이어 확인
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
		if(x >= weight.GetLength(1) || y >= weight.GetLength(0)) { //index out 방지
			return distance;
		}

		distance += weight[y, x]; //현재 위치 가중치 계산
		if (distance <= minWeight) { //distance가 최소보다 작으면 ㄱㄱ/아니라면 가지치기
			distanceStack.Push(distance);
			if (x < weight.GetLength(1) - 1) { //수평 이동
				distance = distanceStack.Peek();
				vectorList.Add(new Vector2(1, 0));
				distance = Dfs(x + 1, y, distance, ref minWeight);
            }
            if(y < weight.GetLength(0) - 1) { //수직 이동
				distance = distanceStack.Peek();
				vectorList.Add(new Vector2(0, 1));
				distance = Dfs(x, y + 1, distance, ref minWeight);
            }

			//플레이어 좌표로 도착했을 때
			vectorList.Add(new Vector2(0, 0));
            if(distance < INF) {
				if (distance < minWeight) {
					minWeight = distance;
					vector = vectorList[0];
				}
				distanceStack.Pop();
				vectorList.RemoveAt(vectorList.Count - 1);

				return distance;
			}
			else {
				distanceStack.Pop();
			}
		}
		vectorList.RemoveAt(vectorList.Count - 1);

		return (minWeight >= INF ? INF : minWeight);
    }

    Vector2 Chasing() {
		int minWeight = INF;
        int distance;
		distanceStack = new Stack<int>();

		//초기화
		GetMapInfo();
		vectorList.Clear();

		distance = Dfs(0, 0, 0, ref minWeight);
		if (distance < INF) {
		}
		else {
			vector = new Vector2(0, 0);
		}
		vector *= plusMinus;

        return vector;
    }

    public void Move() {
		playerPos = GameObject.Find("TestPlayer").transform.position;
		myPos = this.transform.position;
		if(coolTime > 0){
        	coolTime--;
		}
        if(Vector2.Distance(myPos, playerPos) <= 5) { //플레이어 - 몬스터 거리 5 이하면
            if(coolTime <= 0){
                GameManager.Instance.playerTurn = false;
                if(ChaseRange()){
					// 걷는모션 켜주기
                    transform.DOMove((Vector2)transform.position + Chasing(), 0.7f).SetEase(Ease.OutQuad).OnComplete(() => {if(!ChaseRange())Attack(); GameManager.Instance.playerTurn = true; /*걷는모션 꺼주기*/}); //플레이어 추격
                }
                else{
                    Attack();//공격
					GameManager.Instance.playerTurn = true;
                }
            }
        }
    }

    public bool ChaseRange(){
        if(!(transform.position.x - 1 == playerPos.x && transform.position.y == playerPos.y) &&
            !(transform.position.x + 1 == playerPos.x && transform.position.y == playerPos.y) && 
            !(transform.position.x == playerPos.x && transform.position.y - 1 == playerPos.y) &&
            !(transform.position.x == playerPos.x && transform.position.y + 1 == playerPos.y)){
                return true;
        }
        else{
            return false;
        }
    }

    public void Attack(){
        Debug.Log("Attack");
		coolTime += 3;
		//때리는 모션 켜주기
		GameManager.Instance.playerHearts--;
		UIManager.Instance.HeartsSet();
        //때리면 쿨타임 추가;
    }
    
    // private void OnCollisionEnter2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Player")){
    //         Die();//여기 Die 트리거 써서 죽는 애니메이션 나오게 하고 애니메이션 이벤트에 Die넣기
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            Die();//여기 Die 트리거 써서 죽는 애니메이션 나오게 하고 애니메이션 이벤트에 Die넣기
        }
    }

    private void Die(){
        GameManager.Instance.enemies.Remove(gameObject);
        Destroy(gameObject);
        GameManager.Instance.CheckEnemies();
    }
}