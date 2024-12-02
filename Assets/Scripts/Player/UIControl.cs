using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    private void OnInventory() {
        UIManager.Instance.Inventory();
    }
    private void OnInteract(){
        if(GameManager.Instance.ableTP && GameManager.Instance.inTp && SceneManager.GetActiveScene().name == "Lobby"){
            UIManager.Instance.Map();
        }
        else if(GameManager.Instance.ableTP && GameManager.Instance.inTp && SceneManager.GetActiveScene().name != "Lobby"){
            GameManager.Instance.SelectTp();
            if(SceneManager.GetActiveScene().buildIndex == 1){
                GameManager.Instance.map1Value = InventoryManager.Instance.value;
                InventoryManager.Instance.items.Clear();
                InventoryManager.Instance.value = 0;
                InventoryManager.Instance.currentCapacity = 0;
                UIManager.Instance.SetMapBtn();
            }
            else if(SceneManager.GetActiveScene().buildIndex == 2){
                GameManager.Instance.map2Value = InventoryManager.Instance.value;
                InventoryManager.Instance.items.Clear();
                InventoryManager.Instance.value = 0;
                InventoryManager.Instance.currentCapacity = 0;
                UIManager.Instance.SetMapBtn();
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3){
                GameManager.Instance.map3Value = InventoryManager.Instance.value;
                InventoryManager.Instance.items.Clear();
                InventoryManager.Instance.value = 0;
                InventoryManager.Instance.currentCapacity = 0;
                UIManager.Instance.SetMapBtn();
            }
             
            UIManager.Instance.FadeOut();
        }
    }
}
