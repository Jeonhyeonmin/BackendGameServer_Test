using BackEnd;
using System;
using TMPro;
using UnityEngine;

public class FriendBase : MonoBehaviour
{
    [Header("Friend Base")]
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] protected TextMeshProUGUI textTime;  // ����ð�, ���ӽð� ���� �ð� ����

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
        // GetServerTime() - ���� �ð� �ҷ�����
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ� : {callback}");
                return;
            }

            // JSON ������ �Ľ� ����
            try
            {
                // createdAt �ð����κ��� 3�� ���� �ð�
                DateTime after3Days = DateTime.Parse(friendData.createdAt).AddDays(Constants.EXPIRATION_DAYS);
                // ���� ���� �ð�
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
				//{"utcTime":"2024-11-13T00:32:04.248Z"}
				// ������� ���� �ð� = ���� �ð� - ���� ���� �ð�
				TimeSpan timeSpan = after3Days - DateTime.Parse(serverTime);

                // timeSpan.TotalHours�� ���� �Ⱓ�� ��(hour)�� ǥ��
                textTime.text = $"{timeSpan.TotalHours:F0}�ð� ����";
            }

            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
        });
    }
}
