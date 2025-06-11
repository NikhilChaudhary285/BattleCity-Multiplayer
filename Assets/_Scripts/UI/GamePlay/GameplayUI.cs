using UnityEngine;

public class GameplayUI : MonoBehaviour, ISceneUI
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator gameOverAnimator;

    private void Start()
    {
        UIManager.Instance?.RegisterSceneUI(this);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverAnimator.SetTrigger("Show");
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
}
