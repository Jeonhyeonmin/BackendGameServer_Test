using UnityEngine;
using BackEnd;

public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        UpdateMyBestRankData(newScore);
    }

    private void UpdateMyRankData(int newScore)
    {
        string rowInDate = string.Empty;

        // 랭킹 데이터를 업데이트 하려면 게임 데이터에 사용되는 데이터의 inDate 값이 필요
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 조회 중 문제가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

            if (callback.FlattenRows().Count >  0)
            {
                rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다.");
                return;
            }

            Param param = new Param()
            {
                { "dailyBestScore", newScore }
            };

            Backend.URank.User.UpdateUserScore(Constants.DAILT_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"랭킹 등록에 성공했습니다.");
                }
                else
                {
                    Debug.LogError($"랭킹 등록에 실패했습니다. {callback}");
                }
            });
        });
    }

    private void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DAILT_RANK_UUID, callback =>
        {
            if (callback.IsSuccess())
            { 
                // JSON 데이터 파싱 성공
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    // 받아온 데이터의 개수가 0이면 데이터가 없는 것
                    if (rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        // 랭킹을 등록할 때는 컬럼명을 "dailyBestScore"로 저장했지만
                        // 랭킹을 불러올 때는 컬럼명이 score로 통일되어 있다.

                        // 추가로 등록한 항목은 컬럼명을 그대로 사용
                        int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

                        // 현재 점수가 최고 점수보다 높으면
                        if (newScore > bestScore)
                        {
                            // 현재 점수를 새로운 최고 점수로 설정하고, 랭킹에 등록
                            UpdateMyRankData(newScore);

                            Debug.Log($"최고 점수 갱신 {bestScore} -> {newScore}");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            else
            {
                if (callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
                }
            }
        });
    }
}
