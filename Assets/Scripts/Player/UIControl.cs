using UnityEngine;

public class UIControl : MonoBehaviour
{
    private void OnInventory() {
        UIManager.Instance.Inventory();
    }
    private void OnInteract(){
        if(GameManager.Instance.ableTP && GameManager.Instance.inTp){
            UIManager.Instance.Map();
        }
    }
}
