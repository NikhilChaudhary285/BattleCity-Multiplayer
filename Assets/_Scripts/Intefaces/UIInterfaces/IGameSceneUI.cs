public interface IGameSceneUI
{
    void ShowGameOver();
    void ShowGameWin();
    void HideGameOver();
    void HideGameWin();
	void SetWave(int waveNumber);
	void SetEnemyCount(int count);
}
