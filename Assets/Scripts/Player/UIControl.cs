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
            UIManager.Instance.FadeOut();
        }
    }
}
