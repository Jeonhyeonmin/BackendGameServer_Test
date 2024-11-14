using UnityEngine;
using BackEnd;
using NUnit.Framework;
using System.Collections.Generic;

public static class BackendChartData
{
	// 레벨 별 레벨업 필요 경험치와 보상 정보
	public static List<LevelChartData> levelChart;

	static BackendChartData()
	{
		levelChart = new List<LevelChartData>();
	}

	public static void LoadAllChart()
	{
		LoadLevelChart();
	}

	private static void LoadLevelChart()
	{
		Backend.Chart.GetChartContents(Constants.LEVEL_CHART, (callback) =>
		{
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공
				try
				{
					LitJson.JsonData jsonData = callback.FlattenRows();

					// 받아온 데이터의 개수가 0개이면 데이터 존재 X
					if (jsonData.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						for (int i = 0; i < jsonData.Count; ++i)
						{
							LevelChartData newChart = new LevelChartData();
							newChart.level = int.Parse(jsonData[i]["level"].ToString());
							newChart.maxExperience = int.Parse(jsonData[i]["maxExperience"].ToString());
							newChart.rewardGold = int.Parse(jsonData[i]["rewardGold"].ToString());

							levelChart.Add(newChart);

							Debug.Log($"Level : {newChart.level}, Max Exp : {newChart.maxExperience}," + $"Rewrad Gold : {newChart.rewardGold}");
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
				Debug.LogError($"{Constants.LEVEL_CHART}의 차트 불러오기에 에러 발생 {callback}");
			}
		});
	}
}

[System.Serializable]
public class LevelChartData
{
	public int level;
	public int maxExperience;
	public int rewardGold;
}
