using UnityEngine;

public class UIControl : MonoBehaviour
{
    public MonsterMove ms;
    public GameObject monster;
	public GameObject monster2;
	public GameObject monster3;
	public GameObject monster4;
    public MonsterMove ms2;
    public MonsterMove ms3;
    public MonsterMove ms4;

	private void Start() {
        monster = GameObject.Find("TestMonster");
		monster2 = GameObject.Find("Green");
		monster3 = GameObject.Find("Pink");
		monster4 = GameObject.Find("Orange");

		ms = monster.GetComponent<MonsterMove>();
        ms2 = monster2.GetComponent<MonsterMove>();
        ms3 = monster3.GetComponent<MonsterMove>();
        ms4 = monster4.GetComponent<MonsterMove>();
    }

    private void OnInventory() {
        UIManager.Instance.Inventory();
    }

    private void OnEx(){
        Debug.Log("move");
        ms.Move();
        ms2.Move();
        ms3.Move();
        ms4.Move();
    }
}
