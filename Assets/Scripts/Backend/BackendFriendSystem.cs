using BackEnd;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BackendFriendSystem : MonoBehaviour
{
    [SerializeField] private FriendSentRequestPage sentRequestPage;
    [SerializeField] private FriendReceivedRequestPage receivedRequestPage;
    [SerializeField] private FriendPage friendPage;

    private string GetUserInfoBy(string nickname)
    {
        // 해당 닉네임의 유저가 존재하는지 여부는 동기로 진행
        var bro = Backend.Social.GetUserInfoByNickName(nickname);
        string inDate = string.Empty;

        if (!bro.IsSuccess())
        {
            Debug.LogError($"유저 검색 도중 에러가 발생했습니다 {bro}");
            return inDate;
        }

        // JSON 파싱 성공
        try
        {
            LitJson.JsonData jsonData = bro.GetFlattenJSON()["row"];
            Debug.Log(bro.GetReturnValuetoJSON().ToJson());

            // 받아온 데이터의 개수가 0이면 데이터가 없는 것
            if (jsonData.Count <= 0)
            {
                Debug.LogWarning("유저의 inDate 데이터가 없습니다.");
                return inDate;
            }

            inDate = jsonData["inDate"].ToString();

            Debug.Log($"{nickname}의 inDate 값은 {inDate}입니다.");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        return inDate;
    }

    public void SendRequestFriend(string nickname)
    {
        // RequesstFriend() 매서드를 이용해 친구 추가 요청을 할 떄 해당 친구의 inDate 정보가 필요
        string inDate = GetUserInfoBy(nickname);

        // inDate 정보를 가진 유저에게 "친구 요청"을 보낸다.
        Backend.Friend.RequestFriend(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"{nickname} 친구 요청 도중 에러가 발생했습니다. : {callback}");
                return;
            }

            Debug.Log($"친구 요청에 성공했습니다 : {callback}");

            // 친구 요청에 성공하면 친구 요청 대기 목록 불러오기
            GetSentRequestList();
        });
    }

    public void GetSentRequestList()
    {
        Backend.Friend.GetSentRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
             
                Debug.LogError($"친구 요청 대기 목록 조회 도중 오류가 발생했습니다 : {callback}");
                return;
            }
           
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];
				//{"rows":[{"nickname":{"S":"왕자님"},"inDate":{"S":"2024-11-10T11:00:54.370Z"},"createdAt":{"S":"2024-11-12T13:35:52.152Z"}}]}
				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 요청 대기 목록 데이터가 없습니다.");
                    return;
                }

                // 친구 요청 대기 목록에 있는 모든 UI 비활성화
                sentRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    friendData.nickname = item.ContainsKey("nickname") == true ? item["nickname"].ToString() : "NONAME";
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    // [친구 요청]을 보낸 시간으로부터 일정 기간이 지났으면 자동으로 친구 요청 취소
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RevokeSentRequest(friendData.inDate);
                        continue;
                    }

                    sentRequestPage.Activate(friendData);
                }
            }
            catch (System.Exception e)
            {
                // try-catch 에러 출력
                Debug.LogError(e);
            }
        });
    }

    public void RevokeSentRequest(string inDate)
    {
        Backend.Friend.RevokeSentRequest(inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 요청 취소 도중 에러가 발생했습니다. {callback}");
                return;
            }

            Debug.Log($"친구 요청 취소에 성공했습니다.");
        });
    }

    public void GetReceiveRequestList()
    {
        Backend.Friend.GetReceivedRequestList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 수락 대기 목록 조회 도중 에러가 발생했습니다 {callback}");
                return;
            }
            
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];
				//{"rows":[{"nickname":{"S":"왕자님"},"inDate":{"S":"2024-11-10T11:00:54.370Z"},"createdAt":{"S":"2024-11-13T00:26:30.703Z"}},{"nickname":{"NULL":true},"inDate":{"S":"2024-11-12T09:45:45.840Z"},"createdAt":{"S":"2024-11-12T09:45:51.631Z"}},{"nickname":{"NULL":true},"inDate":{"S":"2024-11-12T09:32:26.715Z"},"createdAt":{"S":"2024-11-12T09:32:34.065Z"}}]}
				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 수락 대기 목록 데이터가 없습니다.");
                    return;
                }

                // 친구 수락 대기 목록에 있는 모든 UI 비활성화
                receivedRequestPage.DeactivateAll();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();

                    // 친구 요청을 받은지 3일이 지났다면 자동으로 친구 거절
                    if (IsExpirationDate(friendData.createdAt))
                    {
                        RejectFriend(friendData);
                        continue;
                    }

                    // 현재 friendData 정보를 바탕으로 친구 수락 대기 UI 활성화
                    receivedRequestPage.Activate(friendData);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void AcceptFriend(FriendData friendData)
    {
        Backend.Friend.AcceptFriend(friendData.inDate, callback =>
        {

            if (!callback.IsSuccess())
            {
				Debug.LogError($"친구 수락 중 에러가 발생했습니다 {callback}");
				return;
			}

            Debug.Log($"{friendData.nickname}이(가) 친구가 되었습니다. {callback}");
		}); 
    }

    public void RejectFriend(FriendData friendData)
    {
        Backend.Friend.RejectFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 거절 중 에러가 발생했습니다 {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}의 친구 요청을 거절했습니다. {callback}");
        });
    }

    public void GetFriendList()
    {
        Backend.Friend.GetFriendList(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 목록 조회 도중 에러가 발생했습니다. {callback}");
                return;
            }

            // JSON 파싱
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["rows"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("친구 목록 데이터가 없습니다.");
                    return;
                }

                // 친구 목록에 있는 모든 UI 비활성화
                friendPage.DeactivateAll();

                List<TransactionValue> transactionList = new List<TransactionValue>();
                List<FriendData> friendDataList = new List<FriendData>();

                foreach (LitJson.JsonData item in jsonData)
                {
                    FriendData friendData = new FriendData();

                    friendData.nickname = item["nickname"].ToString().Equals("True") ? "NONAME" : item["nickname"].ToString();
                    friendData.inDate = item["inDate"].ToString();
                    friendData.createdAt = item["createdAt"].ToString();
                    friendData.lastLogin = item["lastLogin"].ToString();

                    friendDataList.Add(friendData);

                    // friendData.InDate를 가지는 친구의 userGameData 정보 불러오기
                    Where where = new Where();
                    where.Equal("owner_inDate", friendData.inDate);
                    transactionList.Add(TransactionValue.SetGet(Constants.USER_DATA_TABLE, where));
                }

                Backend.GameData.TransactionReadV2(transactionList, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError($"Transaction Error : {callback}");
                        return;
                    }

                    LitJson.JsonData userData = callback.GetFlattenJSON()["Responses"];

                    if (userData.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                        return;
                    }

                    for (int i = 0; i < userData.Count; ++i )
                    {
                        // 친구 레벨 정보 설정
                        friendDataList[i].level = $"Lv. {userData[i]["level"]}";
                        // 현재 friendDataList[i] 정보를 바탕으로 친구 UI 활성화
                        friendPage.Activate(friendDataList[i]);
                    }
                });
            }
            // JSON 데이터 파싱 실패
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void BreakFriend(FriendData friendData)
    {
        Backend.Friend.BreakFriend(friendData.inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"친구 삭제 중 에러가 발생했습니다. {callback}");
                return;
            }

            Debug.Log($"{friendData.nickname}와(과) 친구가 해제되었습니다. {callback}");
        });
    }

    private bool IsExpirationDate(string createdAt)
    {
        // GetServerTime() - 서버 시간 불러오기
        var bro = Backend.Utils.GetServerTime();

        if (!bro.IsSuccess())
        {
            Debug.LogError($"서버 시간 불러오기에 실패했습니다. : {bro}");
            return false;
        }

        // JSON 데이터 파싱

        try
        {
            // createdAt 시간으로뷰터 3일 뒤의 시간
            DateTime after3Days = DateTime.Parse(createdAt).AddDays(Constants.EXPIRATION_DAYS);

            string serverTime = bro.GetFlattenJSON()["utcTime"].ToString();
            TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

            if (timeSpan.TotalHours < 0)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }
}
