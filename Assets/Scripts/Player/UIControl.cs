using UnityEngine;

public class UIControl : MonoBehaviour
{
    private void OnInventory() {
        UIManager.Instance.Inventory();
    }
    private void OnInteract(){
        Debug.Log("asasdf");
        GameManager.Instance.Teleport();
    }
}
