using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour, IView
{
	[Header("UI Elements")]
	[SerializeField] private TMP_Text roomNameText;
	[SerializeField] private TMP_Text playerListText;
	[SerializeField] private TMP_Text statusText;
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button leaveButton;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new LobbyPresenter(this);

		startGameButton.onClick.AddListener(() => ((LobbyPresenter)presenter).StartGame());
		leaveButton.onClick.AddListener(() => ((LobbyPresenter)presenter).LeaveRoom());
	}

	public void Dispose()
	{
		startGameButton.onClick.RemoveAllListeners();
		leaveButton.onClick.RemoveAllListeners();
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);

	public void SetRoomName(string roomName)
	{
		roomNameText.text = $"Room: {roomName}";
	}

	public void SetPlayerList(string players)
	{
		playerListText.text = players;
	}

	public void SetStatus(string status)
	{
		statusText.text = status;
	}

	public void SetStartButtonVisible(bool visible)
	{
		startGameButton.gameObject.SetActive(visible);
	}
}