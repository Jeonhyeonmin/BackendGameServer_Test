using System.Collections;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private StageData stageData;   // 경고선, 운석 생성을 위한 스테이지 크기 정보
    [SerializeField] private GameObject alertLinePrefab;
    [SerializeField] private GameObject meteoritePrefab;
    [SerializeField] private float minSpawnCycleTime = 1;
    [SerializeField] private float maxSpawnCycleTime = 4;

	private void Awake()
	{
        StartCoroutine(nameof(Process));
	}

    private IEnumerator Process()
    {
        while (true)
        {
            // 대기 시간 설정
            float spawnCycleTime = Random.Range(minSpawnCycleTime, maxSpawnCycleTime);
            yield return new WaitForSeconds(spawnCycleTime);

            // 경고선 / 운석이 생성되는 x 위치는 스테이지 크기 범위 내에서 임의의 값을 선택
            float x = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

            GameObject alertLineClone = Instantiate(alertLinePrefab, new Vector3(x, 0, 0), Quaternion.identity);

            yield return new WaitForSeconds(1);

            // 경고선 오브젝트 삭제
            Destroy(alertLineClone);

            // 운석 오브젝트 생성 (y 위치는 스테이지 상단 위치 + 1)
            GameObject meteorite = Instantiate(meteoritePrefab, new Vector3(x, stageData.LimitMax.y + 1, 0), Quaternion.identity);
            meteorite.GetComponent<Meteorite>().Setup(gameController);
        }
    }
}
