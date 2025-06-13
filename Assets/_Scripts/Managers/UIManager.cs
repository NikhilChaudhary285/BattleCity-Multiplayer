using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private ISceneUI currentSceneUI;

    public void RegisterSceneUI(ISceneUI sceneUI)
    {
        currentSceneUI = sceneUI;
    }

    public void ShowGameOver()
    {
        currentSceneUI?.ShowGameOver();
    }
    
    public void ShowGameWin()
    {
        currentSceneUI?.ShowGameWin();
    }

    public void HideGameOver()
    {
        currentSceneUI?.HideGameOver();
    } 
    
    public void HideGameWin()
    {
        currentSceneUI?.HideGameWin();
    }

	public void SetWaveText(int wave)
	{
		currentSceneUI?.SetWave(wave); // ISceneUI supports SetWave()
	}
	public void SetEnemyCount(int count)
	{
		currentSceneUI?.SetEnemyCount(count);
	}

	// Add more methods as needed (Pause, Fade, Victory)
}
