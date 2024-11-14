using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Æø¹ß ÀÌÆåÆ® »ý¼º
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gameController.GameOver();
        }
    }
}
