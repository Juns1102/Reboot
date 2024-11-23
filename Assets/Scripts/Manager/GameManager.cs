using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField]
    private bool playerTurn;

    public bool turnCheck() {
        return playerTurn;
    }

    public void turnChange() {
        playerTurn = !playerTurn;
    }
}
