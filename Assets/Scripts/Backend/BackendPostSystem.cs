using UnityEngine;
using BackEnd;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.Events;

public class BackendPostSystem : MonoBehaviour
{
	[System.Serializable]
	public class PostEvent : UnityEvent<List<PostData>> { }
	public PostEvent onGetPostListEvent = new PostEvent();

    private List<PostData> postList = new List<PostData>();

	public void PostListGet()
	{
		PostListGet(PostType.Admin);
	}

	public void PostReceive(PostType postType, string inDate)
	{
		PostReceive(postType, postList.FindIndex(item => item.inDate.Equals(inDate)));	// 서로 다른 매서드
	}

	public void PostReceiveAll()
	{
		PostReceiveAll(PostType.Admin);
	}

	public void PostListGet(PostType postType)
	{
		Backend.UPost.GetPostList(postType, callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"우편 불러오기 중 에러가 발생했습니다. : {callback}");
			}

			Debug.Log($"우편 리스트 불러오기 요청에 성공했습니다.");

			try
			{
				LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];
				//{"postList":[{"content":"우편 내용","expirationDate":"2024-11-12T10:19:57.229Z","reservationDate":"2024-11-11T10:19:57.229Z","nickname":"왕자님","author":"운영자","inDate":"2024-11-11T10:19:57.229Z","title":"우편 제목","sentDate":"2024-11-11T10:19:57.229Z","items":[{"item":{"itemId":"1","itemName":"gold","itemInfo":"게임 내 아이템을 구매하는데 사용하는 재화","chartFileName":"GoodsChart.xlsx"},"itemCount":10900,"chartName":"재화차트"}]}]}

				if (jsonData.Count <= 0)
				{
					Debug.LogWarning("우편함이 비어있습니다.");
					return;
				}

				postList.Clear();

				// 현재 저장 가능한 모든 우편 정보 불러오기
				for (int i = 0; i < jsonData.Count; ++i)
				{
					PostData post = new PostData();

					post.title = jsonData[i]["title"].ToString();
					post.content = jsonData[i]["content"].ToString();
					post.inDate = jsonData[i]["inDate"].ToString();
					post.expirationDate = jsonData[i]["expirationDate"].ToString();

					// 우편에 함계 발송 된 모든 아이템 정보 불러오기
					foreach (LitJson.JsonData itemJson in jsonData[i]["items"])
					{
						// 우편에 함꼐 발송 된 아이템의 차트 이름이 "재화차트" 일 때
						if (itemJson["chartName"].ToString() == Constants.GOODS_CHART_NAME)
						{
							string itemName = itemJson["item"]["itemName"].ToString();
							int itemCount = int.Parse(itemJson["itemCount"].ToString());

							// 우편에 포함 된 아이템이 여러 개 일 때
							// 이미  postReward에 해당 아이템 정보가 있으면 개수 추가
							if (post.postReward.ContainsKey(itemName))
							{
								post.postReward[itemName] += itemCount;
							}
							else
							{
								post.postReward.Add(itemName, itemCount);
							}

							post.isCanReceive = true;
						}
						else
						{
							Debug.LogWarning($"아직 지원하지 않는 차트 정보입니다. : {itemJson["chartName"].ToString()}");
							post.isCanReceive = false;
						}
					}

					postList.Add(post);
				}

				// 우편 리스트 불러오기가 완료되었을 때 이벤트 매서드 호출
				onGetPostListEvent?.Invoke(postList);

				// 저장 가능한 모든 우편 정보 출력
				for (int i = 0; i < postList.Count; ++i)
				{
					Debug.Log($"{i}번 째 우편\n{postList[i].ToString()}");
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(e);
			}
		});
	}

	public void PostReceive(PostType postType, int index)
	{
		if (postList.Count <= 0)
		{
			Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해 주세요.");
			return;
		}

		if (index >= postList.Count)
		{
			Debug.LogError($"해당 우편은 존재하지 않습니다 : 요청 index{index} / 우편 최대 갯수 : {postList.Count}");
		}

		Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편수령을 요청합니다.");

		Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"{postType.ToString()} {postList[index].inDate} 우편수령 중 에러가 발생했습니다. {callback}");
				return;
			}

			Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편수령에 성공했습니다 : {callback}");

			postList.RemoveAt(index);

			if (callback.GetFlattenJSON()["postItems"].Count > 0)
			{
				//{"postItems":[{"item":{"itemId":"0","itemName":"heart","itemInfo":"게임을 플레이할 때 소모","chartFileName":"GoodsChart.xlsx"},"itemCount":14,"chartName":"재화차트"}]}
				SavePostToLocal(callback.GetFlattenJSON()["postItems"]);
				BackendGameData.Instance.GameDataUpdate();
			}
			else
			{
				Debug.LogWarning("수령 가능한 우편 아이템이 존재하지 않습니다.");
			}
		});
	}

	public void PostReceiveAll(PostType postType)
	{
		if (postList.Count <= 0)
		{
			Debug.LogWarning("받을 수 있는 우편이 존재하지 않습니다. 혹은 우편 리스트 불러오기를 먼저 호출해 주세요.");
			return;
		}

		Debug.Log($"{postType.ToString()} 우편 전체 수령을 요청합니다.");

		Backend.UPost.ReceivePostItemAll(postType, callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"{postType.ToString()} 우편 전체 수령 중 에러가 발생했습니다 : {callback}");
				return;
			}

			Debug.Log($"우편 전체 수령에 성공했습니다 : {callback}");
			postList.Clear();
			Debug.Log(callback.GetFlattenJSON());
			// 모든 우편의 아이템 저장
			foreach (LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
			{
				SavePostToLocal(postItemsJson);
			}

			// 플레이어의 재화 정보를 서버에 업데이트
			BackendGameData.Instance.GameDataUpdate();
		});
	}

	public void SavePostToLocal(LitJson.JsonData item)
	{
		//[{"item":{"itemId":"0","itemName":"heart","itemInfo":"게임을 플레이할 때 소모","chartFileName":"GoodsChart.xlsx"},"itemCount":14,"chartName":"재화차트"}]
		try
		{
			foreach (LitJson.JsonData itemJson in item)
			{
				// 차트 파일 이름(*.xlsx)과 Backend Console에 등록한 차트 이름
				string chartFileName = itemJson["item"]["chartFileName"].ToString();
				string chartName = itemJson["chartName"].ToString();

				int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
				string itemName = itemJson["item"]["itemName"].ToString();
				string itemInfo = itemJson["item"]["itemInfo"].ToString();

				int itemCount = int.Parse(itemJson["itemCount"].ToString());

				// 우편으로 받은 재화를 게임 내 데이터에 적용
				if (chartName.Equals(Constants.GOODS_CHART_NAME))
				{
					if (itemName.Equals("heart"))
					{
						BackendGameData.Instance.UserGameData.heart += itemCount;
					}
					else if (itemName.Equals("gold"))
					{
						BackendGameData.Instance.UserGameData.gold += itemCount;
					}
					else if (itemName.Equals("jewel"))
					{
						BackendGameData.Instance.UserGameData.jewel += itemCount;
					}
				}

				Debug.Log($"{chartName} - {chartFileName}");
				Debug.Log($"[{itemId}] {itemName} : {itemInfo}, 획득 수량 : {itemCount}");
				Debug.Log($"아이템을 수령했습니다 : {itemName} - {itemCount}개");
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
		}
	}
}
