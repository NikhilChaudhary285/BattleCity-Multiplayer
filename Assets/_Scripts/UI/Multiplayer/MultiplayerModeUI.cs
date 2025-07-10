using UnityEngine;
using UnityEngine.UI;

public class MultiplayerModeUI : MonoBehaviour, IView
{
	[Header("UI Elements")]
	[SerializeField] private Button createRoomButton;
	[SerializeField] private Button joinRoomButton;
	[SerializeField] private Button backButton;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new MultiplayerModePresenter(this);

		createRoomButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).GoToCreateRoom());
		joinRoomButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).GoToJoinRoom());
		backButton.onClick.AddListener(() => ((MultiplayerModePresenter)presenter).GoBack());
	}

	public void Dispose()
	{
		createRoomButton.onClick.RemoveAllListeners();
		joinRoomButton.onClick.RemoveAllListeners();
		backButton.onClick.RemoveAllListeners();
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);
}