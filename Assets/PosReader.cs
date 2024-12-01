using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PosReader : MonoBehaviour
{
	public GameObject parent;
	Vector2 myPos = parent.GetComponent<Transform>().position;
	Vector2 playerPos = GameObject.Find("TestPlayer").transform.position;
	const int INF = 10000;
	public int[,] weight; //����ġ �迭
	
	//�÷��̾�, ���� ���� ���� �� ���� ����
	void GetMapInfo() {
		myPos = this.transform.position;
		int xDistance = (int)(myPos.x - playerPos.x); //���� - �÷��̾� x�Ÿ�
		int yDistance = (int)(myPos.y - playerPos.y); //���� - �÷��̾� y�Ÿ�
		Debug.Log(xDistance + ", " + yDistance);
		weight = new int[Mathf.Abs(yDistance) + 1, Mathf.Abs(xDistance) + 1]; //�����ϰ� ũ�� -1�� �޾ƹ���.........
		RaycastHit2D hit; //���̾� Ȯ�� ����

		if (yDistance == 0) { //x��
			for (int x = 0; x < Mathf.Abs(xDistance); x++) {
				hit = Physics2D.Raycast(myPos + new Vector2(xDistance > 0 ? -x : x, 0), new Vector2(0, 0), LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
				if (hit.collider == null) { //����(����ġ 1)�� ���
					Debug.Log("Layer: " + hit.collider + " (x, y): (" + yDistance + ", " + xDistance + ")");
					weight[yDistance, x] = 1;
				}
				else { //��ֹ�(����ġ INF) ������ ���
					Debug.Log("Layer: " + hit.collider + " (x, y): (" + yDistance + ", " + xDistance + ")");
					weight[yDistance, x] = INF;
				}
			}
		}
		else if (xDistance == 0) { //y��	
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				hit = Physics2D.Raycast(myPos + new Vector2(0, yDistance > 0 ? -y : y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
				if (hit.collider == null) { //����(����ġ 1)�� ���
					weight[y, 0] = 1;
				}
				else { //��ֹ�(����ġ INF) ������ ���
					weight[y, 0] = INF;
				}
			}
		}
		else if (xDistance < 0 && yDistance < 0) { //�� 1��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
						weight[y, x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (xDistance > 0 && yDistance < 0) { //�� 2��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
						weight[y, x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (xDistance > 0 && yDistance > 0) { //�� 3��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(-x, -y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
						weight[y, x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
						weight[y, x] = INF;
					}
				}
			}
		}
		else if (xDistance < 0 && yDistance > 0) { //�� 4��и�
			for (int y = 0; y < Mathf.Abs(yDistance); y++) {
				for (int x = 0; x < Mathf.Abs(xDistance); x++) {
					hit = Physics2D.Raycast(myPos + new Vector2(x, -y), myPos, LayerMask.GetMask("Platform")); //���� ��ġ ���̾� Ȯ��
					if (hit.collider == null) { //����(����ġ 1)�� ���
						weight[y, x] = 1;
					}
					else { //��ֹ�(����ġ INF) ������ ���
						weight[y, x] = INF;
					}
				}
			}
		}
	}
}
