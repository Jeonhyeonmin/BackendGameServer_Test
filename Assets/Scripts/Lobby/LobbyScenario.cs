using UnityEngine;

public class LobbyScenario : MonoBehaviour
{
    [SerializeField] private UserInfo user;
	[SerializeField] private LevelSystem levelSystem;
	[SerializeField] private TopPanelViewer topPanelViewer;

	private void Awake()
	{
		user.GetUserInfoFromBackend();
	}

	private void Start()
	{
		BackendGameData.Instance.GameDataLoad();
	}

	private void Update()
	{
        if (Input.GetKeyDown("1"))
        {
			levelSystem.Process();
			topPanelViewer.UpdategameData();
        }
    }
}
