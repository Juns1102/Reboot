using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMove : MonoBehaviour {
    const int INF = 10000;
    Vector2 playerPos; //�÷��̾� ��ǥ
    Vector2 myPos; //���� ��ǥ
    RaycastHit2D hit;
    Vector2[][] path;
    int[][] weight;

    //���� �ֺ� �� ������ ����
    //������ ������ ���� �� ã��
    //�ּ� ��� ����

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

        //���� ���� == (����x  - �÷��̾�x) + (����y - �÷��̾�y)
        for(i = 0; i < Mathf.Abs(myPos.x - playerPos.x) + Mathf.Abs(myPos.y - playerPos.y); i++) {
            
        }
	}

	/*
    void shortest_path(GraphType* g, int start) {
        int i, u, w;
        for (i = 0; i<g->n; i++) { �ʱ�ȭ
            distance[i] = g->weight[start][i];
            found[i] = FALSE;
        }
        found[start] = TRUE;    ���� ���� �湮 ǥ��
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
            case 0: //��
				hit = Physics2D.Raycast(myPos, transform.forward, 1, LayerMask.GetMask("Platform"));
				break;
            case 1: //��
				hit = Physics2D.Raycast(myPos, -transform.forward, 1, LayerMask.GetMask("Platform"));
				break;
            case 2: //��
				hit = Physics2D.Raycast(myPos, transform.right, 1, LayerMask.GetMask("Platform"));
				break;
            case 3: //��
				hit = Physics2D.Raycast(myPos, -transform.right, 1, LayerMask.GetMask("Platform"));
				break;
        }
        */
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

    // Update is called once per frame
    void Update() {
		RaycastHit2D hit = Physics2D.Raycast(myPos, new Vector2(0, 0), LayerMask.GetMask("Platform"));
        Debug.Log(hit.collider);
	}
}
