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

    public void HideGameOver()
    {
        currentSceneUI?.HideGameOver();
    }

    // Add more methods as needed (Pause, Fade, Victory)
}
