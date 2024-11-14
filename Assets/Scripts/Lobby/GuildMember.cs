using BackEnd;
using System;
using System.Data;
using TMPro;
using UnityEngine;

public class GuildMember : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPosition;
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textGoodsCount;
    [SerializeField] private TextMeshProUGUI textLastLogin;

    public void Setup(GuildMemberData memberData)
    {
        SetPosition(memberData.position);
        SetDate(memberData.lastLogin);

        textNickname.text = memberData.nickname;
        textGoodsCount.text = memberData.goodsCount.ToString();
    }

    private void SetPosition(string position)
    {
        if (position.Equals("master")) position = "길드마스터";
        else if (position.Equals("viceMaster")) position = "임원";
        else if (position.Equals("member")) position = "길드원";

        textPosition.text = position;
    }

    private void SetDate(string lastLogin)
    {
        // GetServerTime() - 서버 시간 불러오기
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"서버 시간 불러오기에 실패했습니다.");
                return;
            }

            try
            {
                // 현재 서버 시간
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
                // 현재 시간 - 마지막 접속 시간
                TimeSpan timeSpan = DateTime.Parse(serverTime) - DateTime.Parse(lastLogin);

                if (timeSpan.TotalHours < 24) textLastLogin.text = $"{timeSpan.TotalHours:F0}시간 전";
                else textLastLogin.text = $"{timeSpan.TotalDays:F0}일 전";
            }
            catch (Exception ex)
            {
                Debug.LogError (ex);
            }
        });
    }
}
