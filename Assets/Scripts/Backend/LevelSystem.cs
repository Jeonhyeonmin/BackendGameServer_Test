using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private readonly int increaseExperience = 25;   // 게임 플레이 당 획득 경험치

    public void Process()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        BackendGameData.Instance.UserGameData.experience += increaseExperience;

        // 현재 경험치가 최대 경험치보다 크거나 같고, 현재 레벨이 최대 레벨보다 작아 아직 레벨업이 가능할 떄
        if (BackendGameData.Instance.UserGameData.experience >= BackendChartData.levelChart[currentLevel - 1].maxExperience &&
            BackendChartData.levelChart.Count > currentLevel)
        {
            BackendGameData.Instance.UserGameData.gold += BackendChartData.levelChart[currentLevel - 1].rewardGold;
            // 경험치를 0으로 초기화
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        // 게임 정보 업데이트
        BackendGameData.Instance.GameDataUpdate();
        Debug.Log($"현재 레벨 : {BackendGameData.Instance.UserGameData.level}," +
            $"경험치 : {BackendGameData.Instance.UserGameData.experience}/" +
            $"{BackendChartData.levelChart[currentLevel - 1].maxExperience}");
    }
}
