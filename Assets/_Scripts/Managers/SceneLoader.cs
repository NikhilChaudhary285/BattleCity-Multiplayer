using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

	private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

	private void Start()
	{
		// No automatic scene changes here
	}

	public void LoadMainMenu() => StartCoroutine(LoadSceneAsync("MainMenu"));

    public void LoadGame() => StartCoroutine(LoadSceneAsync("Gameplay"));

    public void ReloadCurrentScene()
    {
        string current = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsync(current));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Optional: UIManager.ShowLoadingScreen();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            // Optionally report loading progress
            yield return null;
        }

        // Optional: UIManager.HideLoadingScreen();
    }
	public IEnumerator ReloadSceneAfterDelay(float delay, Scene scene)
	{
		yield return new WaitForSecondsRealtime(delay); // realtime ignores Time.timeScale = 0

		Time.timeScale = 1f; // Reset in case it was paused/frozen

		switch (scene)
		{
			case Scene.MainMenu:
				LoadMainMenu();
				break;

			case Scene.GamePlay:
				LoadGame();
				break;

			default:
				Debug.LogWarning("Unhandled scene type: " + scene);
				break;
		}
	}

}

public enum Scene
{
	MainMenu,
	GamePlay
}

