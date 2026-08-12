using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomUI : MonoBehaviour, IView
{
	[Header("UI Elements")]
	[SerializeField] private TMP_InputField roomNameInput;
	[SerializeField] private TMP_Text statusText;
	[SerializeField] private Button joinButton;
	[SerializeField] private Button backButton;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new JoinRoomPresenter(this);

		joinButton.onClick.AddListener(() => ((JoinRoomPresenter)presenter).JoinRoom(roomNameInput.text));
		backButton.onClick.AddListener(() => ((JoinRoomPresenter)presenter).GoBack());
	}

	public void Dispose()
	{
		joinButton.onClick.RemoveAllListeners();
		backButton.onClick.RemoveAllListeners();
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);

	public void SetStatus(string message)
	{
		statusText.text = message;
	}
}