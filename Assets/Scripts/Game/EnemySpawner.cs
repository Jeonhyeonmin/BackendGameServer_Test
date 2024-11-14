using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private GameController gameController;
    [SerializeField] private StageData stageData;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnCycleTime;

	private void Awake()
	{
		StartCoroutine(nameof(Process));
	}

	private IEnumerator Process()
	{
		int enemyCount = 5;	// 한번에 생성되는 적 숫자
		float distance = 1.2f;	// 생성되는 적 사이의 거리
		float firstX = -2.4f;   // 첫 번째 적의 생성 위치 (왼쪽 끝)

		while (true)
		{
			for (int i = 0; i < enemyCount; ++i)
			{
				Vector3 position = new Vector3(firstX + distance * i, stageData.LimitMax.y + 1, 0);
				GameObject enemey =  Instantiate(enemyPrefab, position, Quaternion.identity);
				enemey.GetComponent<Enemy>().Setup(gameController);
			}

			// spawnCycleTime 시간 동안 대기
			yield return new WaitForSeconds(spawnCycleTime);
		}
	}
}
