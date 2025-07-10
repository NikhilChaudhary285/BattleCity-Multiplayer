using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour, IView
{
	[Header("Text")]
	[SerializeField] private TMP_InputField roomNameInput;
	[SerializeField] private TMP_Text statusText;
	[Header("Buttons")]
	[SerializeField] private Button onePlayerButton;
	[SerializeField] private Button twoPlayerButton;
	[SerializeField] private Button threePlayerButton;
	[SerializeField] private Button fourPlayerButton;
	[SerializeField] private Button createButton;
	[SerializeField] private Button backButton;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new CreateRoomPresenter(this);

		onePlayerButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).SelectPlayerCount(1));
		twoPlayerButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).SelectPlayerCount(2));
		threePlayerButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).SelectPlayerCount(3));
		fourPlayerButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).SelectPlayerCount(4));
		backButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).GoBack());
		createButton.onClick.AddListener(() => ((CreateRoomPresenter)presenter).CreateRoom(roomNameInput.text));
		backButton.onClick.AddListener(() => ((CreateRoomPresenter)presenter).GoBack());
	}

	public void Dispose()
	{
		onePlayerButton.onClick.RemoveAllListeners();
		twoPlayerButton.onClick.RemoveAllListeners();
		threePlayerButton.onClick.RemoveAllListeners();
		fourPlayerButton.onClick.RemoveAllListeners();
		createButton.onClick.RemoveAllListeners();
		backButton.onClick.RemoveAllListeners();
	}

	public void SetStatus(string message)
	{
		statusText.text = message;
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);
}