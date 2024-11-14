using UnityEngine;

public class LogoScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;
	[SerializeField] private SceneNames nextScene;
	private void Awake()
	{
		SystemSetup();
	}

	private void SystemSetup()
	{
		Application.runInBackground = true;

		// �ػ� ���� (9:18.5, 1440x2960)
		int width = Screen.width;
		int height = (int)(Screen.width * 18.5f / 9);
		Screen.SetResolution(width, height, true);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// �ε� �ִϸ��̼� ����, ��� �Ϸ� �� OnAfterProgress() �ż��� ����

		progress.Play(OnAfterProgress);
	}

	private void OnAfterProgress()
	{
		Utils.LoadScene(nextScene);
	}
}
