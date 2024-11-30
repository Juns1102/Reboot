using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Cecil;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    Vector2 playerPos; //�÷��̾� ��ǥ
    Vector2 myPos; //���� ��ǥ
    int[][] weight; //����ġ �迭
    

    //���� �ֺ� �� ������ ����
    //������ ������ ���� �� ã��
    //�ּ� ��� ����

    //�÷��̾�, ���� ���� ���� �� ���� ����
    void GetMapInfo() {
        Array.Clear(weight, 0, weight.Length); //����ġ �迭 �ʱ�ȭ(������ ������ ����, �� �ٲ��� �ʱ�ȭ �������)
        float xDistance = Mathf.Abs(myPos.x - playerPos.x); //�÷��̾� - ���� x�Ÿ�
        float yDistance = Mathf.Abs(myPos.y - playerPos.y); //�÷��̾� - ���� y�Ÿ�
        RaycastHit2D hit; //���̾� Ȯ�� ����

        for(int i = 0; i < xDistance; i++) {
            for(int j = 0; j < yDistance; j++) {
                hit = Physics2D.Raycast(myPos, myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
                if(hit.collider == null) { //����(����ġ 1)�� ���
                    weight[i][j] = 1;
                }
                else { //��ֹ�(����ġ INF) ������ ���
                    weight[i][j] = INF;
                }
            }
        }
    }

    //���� �̵�(x��ǥ, y��ǥ, ����ġ ��, �ּ� ����ġ)
    int MoveHorizontal(int x, int y, int distance, int minWeight) {
        //����ġ �� < �ּҶ�� ����
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

		return minWeight; //����ġ �� >= �ּ��� ���
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
		//�ʱ�ȭ
		GetMapInfo();

        Dfs((int)myPos.x, (int)myPos.y, 0, minWeight);

        return myPos; //�ӽ� ��ȯ
    }

	void Wandering() {
    }

    void Move() {
        myPos = this.transform.position;
        if(Vector2.Distance(myPos, playerPos) <= 5) { //�÷��̾� - ���� �Ÿ� 5 ���ϸ�
            Chasing(); //�÷��̾� �߰�
        }
        else {
            Wandering(); //���� �̵�
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        playerPos = GameObject.Find("TestPlayer").transform.position;
        myPos = this.transform.position;
    }
}
