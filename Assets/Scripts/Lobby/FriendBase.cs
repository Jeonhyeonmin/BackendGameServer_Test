using BackEnd;
using System;
using TMPro;
using UnityEngine;

public class FriendBase : MonoBehaviour
{
    [Header("Friend Base")]
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] protected TextMeshProUGUI textTime;  // 만료시간, 접속시간 등의 시간 정보

    protected BackendFriendSystem backendFriendSystem;
    protected FriendPageBase friendPageBase;
    protected FriendData friendData;

    public virtual void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
    {
        backendFriendSystem = friendSystem;
        friendPageBase = friendPage;
        this.friendData = friendData;

        textNickname.text = friendData.nickname;
    }

    public void SetExpirationDate()
    {
        // GetServerTime() - 서버 시간 불러오기
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"서버 시간 불러오기에 실패했습니다 : {callback}");
                return;
            }

            // JSON 데이터 파싱 성공
            try
            {
                // createdAt 시간으로부터 3일 뒤의 시가
                DateTime after3Days = DateTime.Parse(friendData.createdAt).AddDays(Constants.EXPIRATION_DAYS);
                // 현재 서버 시간
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
				//{"utcTime":"2024-11-13T00:32:04.248Z"}
				// 만료까지 남은 시간 = 만료 시간 - 현재 서버 시간
				TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

                // timeSpan.TotalHours로 남은 기간을 시(hour)로 표현
                textTime.text = $"{timeSpan.TotalHours:F0}시간 남음";
            }

            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
        });
    }
}
