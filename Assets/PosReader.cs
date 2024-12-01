using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PosReader : MonoBehaviour
{
	public GameObject parent;
	Vector2 myPos = parent.GetComponent<Transform>().position;
	Vector2 playerPos = GameObject.Find("TestPlayer").transform.position;
	const int INF = 10000;
	public int[,] weight; //가중치 배열
	
	//플레이어, 몬스터 사이 범위 맵 정보 수집
	void GetMapInfo() {
		myPos = this.transform.position;
		int xDistance = (int)(myPos.x - playerPos.x); //몬스터 - 플레이어 x거리
		int yDistance = (int)(myPos.y - playerPos.y); //몬스터 - 플레이어 y거리
		Debug.Log(xDistance + ", " + yDistance);
		weight = new int[Mathf.Abs(yDistance) + 1, Mathf.Abs(xDistance) + 1]; //몽총하게 크기 -1로 받아버림.........
		RaycastHit2D hit; //레이어 확인 변수

		if (yDistance == 0) { //x축
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
		else if (xDistance == 0) { //y축	
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
		else if (xDistance < 0 && yDistance < 0) { //제 1사분면
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
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
		else if (xDistance > 0 && yDistance < 0) { //제 2사분면
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
		else if (xDistance > 0 && yDistance > 0) { //제 3사분면
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
		else if (xDistance < 0 && yDistance > 0) { //제 4사분면
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
}
