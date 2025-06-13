using UnityEngine;

public class GameWinState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("State: GameWin");

        UIManager.Instance?.ShowGameWin();
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting GameOver...");
    }
}
