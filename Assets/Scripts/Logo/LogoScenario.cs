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

		// 해상도 설정 (9:18.5, 1440x2960)
		int width = Screen.width;
		int height = (int)(Screen.width * 18.5f / 9);
		Screen.SetResolution(width, height, true);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// 로딩 애니메이션 시작, 재생 완료 시 OnAfterProgress() 매서드 실행

		progress.Play(OnAfterProgress);
	}

	private void OnAfterProgress()
	{
		Utils.LoadScene(nextScene);
	}
}
