using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [Header("InGame")]
    [SerializeField] private TextMeshProUGUI textScore;

	[Header("Game Over")]
	[SerializeField] private GameObject panelGameOver;
	[SerializeField] private TextMeshProUGUI textResultScore;

	[Header("Game Over UI Animation")]
	[SerializeField] private ScaleEffect effectGameOver;
	[SerializeField] private CountingEffect effectResultScore;

	private void Update()
	{
		textScore.text = $"SCORE {gameController.Score}";
	}

	public void OnGameOver()
	{
		panelGameOver.SetActive(true);
		textResultScore.text = gameController.Score.ToString();

		// "GAME OVER" 텍스트 크기 축소 애니메이션
		effectGameOver.Play(200, 100);
		// 0 -> gameController.Score까지 점수를 카운팅하는 애니메이션
		effectResultScore.Play(0, gameController.Score);
	}

	public void btnClickGoToLobby()
	{
		Utils.LoadScene(SceneNames.Lobby);
	}
}
