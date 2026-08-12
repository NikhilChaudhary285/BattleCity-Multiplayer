using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private IGameSceneUI currentGameSceneUI;
    private IMainMenuUI currentMainMenuUI;

    public void RegisterSceneUI(IGameSceneUI sceneUI)
    {
        currentGameSceneUI = sceneUI;
    }
    
    public void RegisterMainMenuUI(IMainMenuUI sceneUI)
    {
		currentMainMenuUI = sceneUI;
    }

    public void ShowGameOver()
    {
        currentGameSceneUI?.ShowGameOver();
    }
    
    public void ShowGameWin()
    {
        currentGameSceneUI?.ShowGameWin();
    }

    public void HideGameOver()
    {
        currentGameSceneUI?.HideGameOver();
    } 
    
    public void HideGameWin()
    {
        currentGameSceneUI?.HideGameWin();
    }

	public void SetWaveText(int wave)
	{
		currentGameSceneUI?.SetWave(wave); // ISceneUI supports SetWave()
	}
	public void SetEnemyCount(int count)
	{
		currentGameSceneUI?.SetEnemyCount(count);
	}

	public void ShowGameModePanel() => currentMainMenuUI.ShowGameModeUI();
	public void ShowCreateRoomPanel() => currentMainMenuUI.ShowCreateRoomUI();
	public void ShowJoinRoomPanel() => currentMainMenuUI.ShowJoinRoomUI();
	public void ShowMultiplayerPanel() => currentMainMenuUI.ShowMultiplayerModeUI();
	public void ShowShareRoomPanel() => currentMainMenuUI.ShowShareRoomLobbyUI();
	public void HideAllPanels() => currentMainMenuUI.HideAllPanels();


	// Add more methods as needed (Pause, Fade, Victory)
}
