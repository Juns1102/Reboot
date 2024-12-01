using UnityEngine;

public class UIControl : MonoBehaviour
{
    public MonsterMove ms;
    public GameObject monster;

    private void Start() {
        monster = GameObject.Find("TestMonster");
        ms = monster.GetComponent<MonsterMove>();
    }

    private void OnInventory() {
        UIManager.Instance.Inventory();
    }

    private void OnEx(){
        Debug.Log("move");
        ms.Move();
    }
}
