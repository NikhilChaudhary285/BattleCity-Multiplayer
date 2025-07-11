using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShareRoomLobbyUI : MonoBehaviour, IView
{ 
	[Header("UI Elements")]

	[Header("ShareRoom UI")]
	[SerializeField] private TMP_Text roomNameText;
	[SerializeField] private Button startGameButton;
	[SerializeField] private Button leaveButton;
	[Header("ShareRoomFriends UI")]
	[SerializeField] private RectTransform FriendRoomShareContainer;
	[SerializeField] private RectTransform NoFriendsYetContainer;
	[SerializeField] private GameObject FriendRoomSharePrefab;
	[Header("JoinedRoomFriends UI")]
	[SerializeField] private TMP_Text statusText;
	[SerializeField] private RectTransform JoinedFriendsListContainer;
	[SerializeField] private RectTransform NoComradesYet_Container;
	[SerializeField] private GameObject JoinedFriendPrefab;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new ShareRoomLobbyPresenter(this);

		startGameButton.onClick.AddListener(() => ((ShareRoomLobbyPresenter)presenter).StartGame());
		leaveButton.onClick.AddListener(() => ((ShareRoomLobbyPresenter)presenter).LeaveRoom());
	}

	public void Dispose()
	{
		startGameButton.onClick.RemoveAllListeners();
		leaveButton.onClick.RemoveAllListeners();
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);
	public void ToggleJoinedComradesContainerByPlayerCount(int PlayerCount)
	{
		bool hasPlayers = PlayerCount > 0;

		NoComradesYet_Container.gameObject.SetActive(!hasPlayers);
		JoinedFriendsListContainer.gameObject.SetActive(hasPlayers);
	}

	public void SetRoomName(string roomName)
	{
		roomNameText.text = $"Room: {roomName}";
	}

	public void SetPlayerList(List<JoinedRoomFriendData> joinedRoomFriends)
	{
		// Clear previous children from the container
		foreach (Transform child in JoinedFriendsListContainer.transform)
		{
			Destroy(child.gameObject);
		}

		// Instantiate new friend entries
		foreach (JoinedRoomFriendData joinedRoomFriend in joinedRoomFriends)
		{
			GameObject roomFriend = Instantiate(FriendRoomSharePrefab, JoinedFriendsListContainer);
			// Set roomFriend data on the prefab (e.g., username, wins, userprofile etc.)
			roomFriend.GetComponent<Friend>().userName_Text.text = joinedRoomFriend.userName;
			roomFriend.GetComponent<Friend>().wins_Text.text = joinedRoomFriend.wins.ToString();
			roomFriend.GetComponent<Friend>().userProfile_Sprite = joinedRoomFriend.userProfile;
		}
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