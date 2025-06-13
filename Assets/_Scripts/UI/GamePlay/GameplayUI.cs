using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour, ISceneUI
{
    [Header("PANEL")]
    [Tooltip("GameOver Panel")] [SerializeField] private GameObject gameOverPanel;
	[Tooltip("GameWin Panel")] [SerializeField] private GameObject gameWinPanel;

    [Header("TEXT")]
	[Tooltip("Wave Status Text")] [SerializeField] private TextMeshProUGUI waveStatusText;

	private int currentWave;
	private int currentEnemyCount;

	public void SetWave(int waveNumber)
	{
		currentWave = waveNumber;
		UpdateWaveStatusText();
	}

	public void SetEnemyCount(int count)
	{
		currentEnemyCount = count;
		UpdateWaveStatusText();
	}

	private void UpdateWaveStatusText()
	{
		waveStatusText.text = $"Wave: {currentWave} — Enemies Left: {currentEnemyCount}";
	}

	private void Awake()
    {
        UIManager.Instance?.RegisterSceneUI(this);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    } 
	
	public void ShowGameWin()
    {
        gameWinPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    } 
	
	public void HideGameWin()
    {
        gameWinPanel.SetActive(false);
    }
}
