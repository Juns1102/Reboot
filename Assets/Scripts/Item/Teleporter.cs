using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private int teleportNum;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            GameManager.Instance.SetTp(teleportNum);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            GameManager.Instance.SetTp(0);
        }
    }
}

