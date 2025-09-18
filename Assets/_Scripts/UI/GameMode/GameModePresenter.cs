using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModePresenter : IPresenter
{
    private GameModeUI view;

    public GameModePresenter(GameModeUI view)
    {
        this.view = view;
        Initialize();
    }

    public void Initialize()
    {
        Debug.Log("[GameModePresenter] Initialized.");
    }

    public void Dispose()
    {
        Debug.Log("[GameModePresenter] Disposed.");
    }

    public void OnClickOffline()
    {
        Debug.Log("[GameModePresenter] Offline mode selected.");
        GameSettingsManager.Instance.SetMode(GameMode.SinglePlayer, 1);

        // For offline we load the Gameplay scene locally (no Photon)
        // This ensures NetworkGameManager will spawn a local player
        SceneManager.LoadScene("GamePlay"); // make sure Scene name matches the constant used in NetworkGameManager
    }

    public void OnClickOnline()
    {
        Debug.Log("[GameModePresenter] Online mode selected.");
        UIManager.Instance.ShowMultiplayerPanel(); // next panel
    }

    public void GoBack()
    {
        // UIManager.Instance.ShowMultiplayerModeUI(); // Just Example Set For Now Bcz we didn't have any Panel from GameModePanel TOGOTO Other
    }
}
