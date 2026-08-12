using UnityEngine;

public class MultiplayerModePresenter : IPresenter
{
	private MultiplayerModeUI view;

	public MultiplayerModePresenter(MultiplayerModeUI view)
	{
		this.view = view;
		Initialize();
	}

	public void Initialize()
	{
		Debug.Log("[MultiplayerModePresenter] Initialized");
	}

	public void Dispose()
	{
		Debug.Log("[MultiplayerModePresenter] Initialized");
	}

	public void GoToCreateRoom()
	{
		UIManager.Instance.ShowCreateRoomPanel();
	}

	public void GoToJoinRoom()
	{
		UIManager.Instance.ShowJoinRoomPanel();
	}

	public void GoBack()
	{
		UIManager.Instance.ShowGameModePanel();
	}
}