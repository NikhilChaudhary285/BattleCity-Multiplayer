using UnityEngine;

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

		SceneLoader.Instance.LoadGame(); // load gameplay scene
	}

	public void OnClickOnline()
	{
		Debug.Log("[GameModePresenter] Online mode selected.");
		UIManager.Instance.ShowMultiplayerModeUI(); // next panel
	}

	public void GoBack()
	{
		// UIManager.Instance.ShowMultiplayerModeUI(); // Just Example Set For Now Bcz we didn't have any Panel from GameModePanel TOGOTO Other
	}
}