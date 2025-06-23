using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour, IView
{
	[Header("UI References")]
	[SerializeField] private TMP_InputField roomInputField;
	[SerializeField] private TMP_Text statusText;
	[SerializeField] private TMP_Text playerListText;
	[SerializeField] private Button joinRoomButton;
	[SerializeField] private Button startGameButton;

	public event Action<string> OnJoinRoomPressed;
	public event Action OnStartGamePressed;

	public void Initialize()
	{
		joinRoomButton.onClick.AddListener(() => OnJoinRoomPressed?.Invoke(roomInputField.text));
		startGameButton.onClick.AddListener(() => OnStartGamePressed?.Invoke());
	}

	public void SetStatus(string message) => statusText.text = message;
	public void SetPlayerList(string list) => playerListText.text = list;
	public void SetStartButtonVisible(bool visible) => startGameButton.gameObject.SetActive(visible);
}