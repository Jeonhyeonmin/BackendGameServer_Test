using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int scorePoint = 100;  // 적을 처치했을 때 획득하는 점수
    [SerializeField] private GameObject explosionPrefab;
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnDie()
    {
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		gameController.Score += scorePoint; // 플레이어의 점수를 socrePoint만큼 증가
		Destroy(gameObject);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
        {
            OnDie();
            gameController.GameOver();
        }
	}
}
