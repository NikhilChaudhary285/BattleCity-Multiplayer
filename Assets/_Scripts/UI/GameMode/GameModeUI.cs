using UnityEngine;
using UnityEngine.UI;

public class GameModeUI : MonoBehaviour, IView
{
	[Header("UI Elements")]
	[SerializeField] private Button offlineButton;
	[SerializeField] private Button onlineButton;
	[SerializeField] private Button backButton;

	private IPresenter presenter;

	public void Initialize()
	{
		presenter = new GameModePresenter(this);

		offlineButton.onClick.AddListener(() => ((GameModePresenter)presenter).OnClickOffline());
		onlineButton.onClick.AddListener(() => ((GameModePresenter)presenter).OnClickOnline());
		backButton.onClick.AddListener(() => ((GameModePresenter)presenter).GoBack());
	}

	public void Dispose()
	{
		offlineButton.onClick.RemoveAllListeners();
		onlineButton.onClick.RemoveAllListeners();
		backButton.onClick.RemoveAllListeners();
	}

	public void Show() => gameObject.SetActive(true);
	public void Hide() => gameObject.SetActive(false);
}