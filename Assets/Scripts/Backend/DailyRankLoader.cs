using BackEnd;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class DailyRankLoader : MonoBehaviour
{
	[SerializeField] private GameObject rankDataPrefab; // ŷ ���� ����� ���� UI ���α׷�
	[SerializeField] private Scrollbar scrollbar;   // scrollBar�� value ���� (Ȱ��ȭ �� �� 1���� ���̵���)
	[SerializeField] private Transform rankDataParent;  // ScrollView�� Content ������Ʈ
	[SerializeField] private DailyRankData myRankData;  // �� ��ŷ ������ ����ϴ� UI ���� ������Ʈ

	private List<DailyRankData> rankDataList;

	private void Awake()
	{
		rankDataList = new List<DailyRankData>();

		// 1 ~ 20 ��ŷ ����� ���� UI ������Ʈ ����
		for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
		{
			GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
			rankDataList.Add(clone.GetComponent<DailyRankData>());
		}
	}

	private void OnEnable()
	{
		// 1�� ��ŷ�� ���̵��� scroll �� ����
		scrollbar.value = 1;
		// 1~ 20���� ��ŷ ���� �ҷ�����
		GetRankList();
		// �� ��ŷ ���� �ҷ�����
		GetMyRank();
	}

	private void GetRankList()
	{
		// 1 ~ 20 �� ��ŷ ���� �ҷ�����
		Backend.URank.User.GetRankList(Constants.DAILT_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
		{
			if (callback.IsSuccess())
			{
				try
				{
					Debug.Log($"��ŷ ��ȸ�� �����߽��ϴ�. : {callback}");

					LitJson.JsonData rankDataJson = callback.FlattenRows();

					// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
					if (rankDataJson.Count <= 0)
					{
						for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
						{
							SetRankData(rankDataList[i], i + 1, "-", 0);
						}

						Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
					}
					else
					{
						int rankerCount = rankDataJson.Count;

						// ��ŷ ���� �ҷ��� ����� �� �ֵ��� ����
						for (int i = 0; i < rankerCount; ++i)
						{
							rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
							rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

							// �г����� ������ �������� ���� ������ ������ �� �ֱ� ������
							// �г����� �������� �ʴ� ������ �г��� ��� gamerID�� ���
							rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ? rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
						}

						// ���� rankerCount�� 20������ �������� ������ ������ ��ŷ���� �� �����͸� ����
						for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
						{
							SetRankData(rankDataList[i], i + 1, "-", 0);
						}
					}
				}
				catch (System.Exception e)
				{
					Debug.LogError(e);
				}
			}
			else
			{
				for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
				{
					SetRankData(rankDataList[i], i + 1, "-", 0);
				}

				Debug.LogError($"��ŷ ��ȸ �� ������ �߻��߽��ϴ�. {callback}");
			}
		});
	}

	private void GetMyRank()
	{
		// �� ��ŷ ���� �ҷ�����
		Backend.URank.User.GetMyRank(Constants.DAILT_RANK_UUID, callback =>
		{
			// �г����� ������ gamerID, �г����� ������ nickname ���
			string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

			if (callback.IsSuccess())
			{
				try
				{
					LitJson.JsonData rankDataJson = callback.FlattenRows();

					// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
					if (rankDataJson.Count <= 0)
					{
						// ["������ ����", "�г���", 0]�� ���� ���
						SetRankData(myRankData, 100000000, nickname, 0);

						Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
					}
					else
					{
						myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
						myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

						myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
						rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
					}
				}
				catch (System.Exception ex)
				{
					// ["������ ����", "�г���", 0]�� ���� ���
					SetRankData(myRankData, 100000000, nickname, 0);

					Debug.LogError(ex);
				}
			}
			else
			{
				// �ڽ��� ��ŷ ������ �������� ���� ��
				if (callback.GetMessage().Contains("userRank"))
				{
					SetRankData(myRankData, 100000000, nickname, 0);
				}
			}
		});
	}

	private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
	{
		rankData.Rank = rank;
		rankData.Nickname = nickname;
		rankData.Score = score;
	}
}
