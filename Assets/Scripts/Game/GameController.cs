using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] UnityEvent onGameOver; // 게임 오버 되었을 때 호출할 매서드
    [SerializeField] private DailyRankRegister dailyRank; 

    private int score = 0;
    public bool IsGameOver { set; get; } = false;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    public void GameOver()
    {
        // 중복 처리 되지 않도록 bool 변수로 제어

        if (IsGameOver == true)
        {
            return;
        }

        IsGameOver = true;

        // 게임오버 되었을 때 호출할 매서드들을 실행
        onGameOver.Invoke();

        // 현재 점수 정보를 바탕으로 랭킹 데이터 갱신
        dailyRank.Process(score);
    }
}
