using UnityEngine;
using LitJson;
using UnityEngine.Events;
using BackEnd;

public class UserInfo : MonoBehaviour
{
    [System.Serializable]
    public class UserInfoEvent : UnityEvent { }

	public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

    private static UserInfoData data = new UserInfoData();
    public static UserInfoData Data => data;

    public void GetUserInfoFromBackend()
    {
        // ���� �α����� ����� ���� �ҷ�����
        Backend.BMember.GetUserInfo(callback =>
        {
            /// ���� �ҷ����� ����
            if (callback.IsSuccess())
            {
                // JSON ������ �Ľ� ����
                try
                {
                    JsonData json = callback.GetReturnValuetoJSON()["row"];
                    data.gamerId = json["gamerId"].ToString();
                    data.countryCode = json["countryCode"]?.ToString();
                    data.nickname = json["nickname"]?.ToString();
                    data.inDate = json["inDate"].ToString();
                    data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
                    data.subscriptionType = json["subscriptionType"].ToString();
                    data.federationId = json["federationId"]?.ToString();
                }
                catch(System.Exception ex)
                {
                    data.Reset();
                    Debug.LogError(ex);
                }
            }
            else
            {
                // ���� ������ �⺻ ���·� ����
                // Tip. �Ϲ������� �������� ���¸� ����� �⺻���� ������ �����صΰ� ���������� �� ���
                data.Reset();
                Debug.LogError(callback.GetMessage());
            }

            // ���� ���� �ҷ����� �������� �� onUserInfoEvent�� ��ϵǾ� �ִ� �̺�Ʈ �ż��� ȣ��
            onUserInfoEvent?.Invoke();
        });
    }
}

public class UserInfoData
{
    public string gamerId;
    public string countryCode;  // �����ڵ� ���� �������� null
    public string nickname; // �г��� ���� �������� null
    public string inDate;   // ������ inDate
    public string emailForFindPassword; // �̸��� �ּ�, ���� �������� null
    public string subscriptionType; // Ŀ����, ������̼� Ÿ��
    public string federationId; // ����, ����, ���̽���, ������̼� ID, Ŀ���� ������ null

    public void Reset()
    {
        gamerId = "Offline";
        countryCode = "Unknown";
        nickname = "Noname";
        inDate = string.Empty;
        emailForFindPassword = string.Empty;
        subscriptionType = string.Empty;
        federationId = string.Empty;
    }
}
