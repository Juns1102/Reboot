using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

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

    int MoveHorizontal(int x, int minWeight) {
        //����ġ �� < �ּҶ�� ����
        if(x < playerPos.x) {
            MoveHorizontal(x + 1, minWeight);
        }
        else if(x > playerPos.x) {
            MoveHorizontal(x - 1, minWeight);
        }

        return weight[x][x]; //�ӽ� ��ȯ
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
        //�ʱ�ȭ
        GetMapInfo();

        Dfs();

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
