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
    List<Vector2> vectorList;
    Vector2 vector;

    //���� �ֺ� �� ������ ����
    //������ ������ ���� �� ã��
    //�ּ� ��� ����

    //�÷��̾�, ���� ���� ���� �� ���� ����
    void GetMapInfo() {
        Array.Clear(weight, 0, weight.Length); //����ġ �迭 �ʱ�ȭ(������ ������ ����, �� �ٲ��� �ʱ�ȭ �������)
        int xDistance = (int)(myPos.x - playerPos.x); //���� - �÷��̾� x�Ÿ�
        int yDistance = (int)(myPos.y - playerPos.y); //���� - �÷��̾� y�Ÿ�

        RaycastHit2D hit; //���̾� Ȯ�� ����

        if(yDistance == 0) { //x��
			for (int x = 0; x < Mathf.Abs(xDistance); x++) {
				hit = Physics2D.Raycast(myPos + new Vector2(xDistance > 0 ? -x : x, 0), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
				if (hit.collider == null) { //����(����ġ 1)�� ���
					weight[0][x] = 1;
				}
				else { //��ֹ�(����ġ INF) ������ ���
					weight[0][x] = INF;
				}
			}
		}
        else if(xDistance == 0) { //y��
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				hit = Physics2D.Raycast(myPos + new Vector2(0, yDistance > 0 ? -y : y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
				if (hit.collider == null) { //����(����ġ 1)�� ���
					weight[y][0] = 1;
				}
				else { //��ֹ�(����ġ INF) ������ ���
					weight[y][0] = INF;
				}
			}
		}
        else if(xDistance < 0 && yDistance < 0) { //�� 1��и�
            for (int y = 0; y < Mathf.Abs(yDistance); y++) {
                for(int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
                        weight[y][x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
                        weight[y][x] = INF;
					}
				}
            }
        }
        else if(xDistance > 0 && yDistance < 0) { //�� 2��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
                        weight[y][x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
                        weight[y][x] = INF;
					}
				}
			}
		}
        else if(xDistance > 0 && yDistance > 0) { //�� 3��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, -y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
                        weight[y][x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
                        weight[y][x] = INF;
					}
				}
			}
		}
        else if(xDistance < 0 && yDistance > 0) { //�� 4��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, -y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
						weight[y][x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
						weight[y][x] = INF;
					}
				}
			}
		}
    }

    int Dfs(int x, int y, int distance, ref int minWeight) {
        if(distance < minWeight) {
			if(x < weight[y].Length - 1) { //���� �̵�
                vectorList.Add(new Vector2(1, 0));
				distance = Dfs(x + 1, y, distance + weight[y][x], ref minWeight);
            }
            if(y < weight.Length - 1) { //���� �̵�
                vectorList.Add(new Vector2(0, 1));
				distance = Dfs(x, y + 1, distance + weight[y][x], ref minWeight);
            }

            if(distance < minWeight) {
                minWeight = distance;
                vector = vectorList[0];
            }
            vectorList.RemoveAt(vectorList.Count - 1);
		}

        return distance;
    }

    Vector2 Chasing() {
		int minWeight = INF;
        int distance = 0;

		//�ʱ�ȭ
		GetMapInfo();

        distance = Dfs(0, 0, 0, ref minWeight);
        Debug.Log("Shortest: " + distance); //�ִ� �Ÿ� Ȯ�ο� �α� ���

        return vector;
    }

	void Wandering() {
    }

    void Move() {
		playerPos = GameObject.Find("TestPlayer").transform.position;
		myPos = this.transform.position;
        if(Vector2.Distance(myPos, playerPos) <= 5) { //�÷��̾� - ���� �Ÿ� 5 ���ϸ�
            Chasing(); //�÷��̾� �߰�
        }
        else {
            Wandering(); //���� �̵�
        }
    }
}
