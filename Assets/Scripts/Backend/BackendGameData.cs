using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowInDate = string.Empty;

    /// <summary>
    /// �ڳ� �ܼ� ���̺� ���ο� ���� ���� �߰�
    /// </summary>
    public void GameDataInsert()
    {
        // ���� ������ �ʱⰪ���� ����
        userGameData.Reset();

        // ���̺� �߰��� �����ͷ� ����
        Param param = new Param()
        {
            { "level", userGameData.level },
            { "experience", userGameData.experience },
            { "gold", userGameData.gold },
            { "jewel", userGameData.jewel },
            { "heart", userGameData.heart },
            { "dailyBestScore", userGameData.dailyBestScore }
        };

        // ù ��° �Ű������� �ڳ� �ܼ��� "���� ���� ����" �ǿ� ������ ���̺� �̸�
        Backend.GameData.Insert("USER_DATA", param, callback =>
        {
            // ���� ���� �߰��� �������� ��
            if (callback.IsSuccess())
            {
                gameDataRowInDate = callback.GetInDate();

                Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ�. : {callback}");
            }
            else
            {
                Debug.LogError($"���� ���� ������ ���Կ� �����߽��ϴ�. : {callback}");
            }
        });
    }

    // �ڳ� �ܼ� ���̺��� ���� ������ �ҷ��� �� ȣ��
    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            // ���� ���� �ҷ����⿡ �������� ��,
            if (callback.IsSuccess())
            {
                Debug.Log($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");

                // JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();
					//[{"experience":100,"client_date":"2024-11-10T11:00:29.774Z","level":4,"inDate":"2024-11-10T11:00:55.581Z","updatedAt":"2024-11-12T06:28:20.845Z","heart":579,"jewel":2468,"owner_inDate":"2024-11-10T11:00:54.370Z","dailyBestScore":0,"gold":190342}]

					// �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
					if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
					else
					{
                        // �ҷ��� ���� ������ ������
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
						// �ҷ��� ���� ������ usergameData ������ ����
						userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
						userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
						userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
						userGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
						userGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());
                        userGameData.dailyBestScore = int.Parse(gameDataJson[0]["dailyBestScore"].ToString());

						onGameDataLoadEvent?.Invoke();
					}
				}

                // Json ������ �Ľ� ����
                catch (System.Exception e)
                {
                    // ���� ������ �ʱⰪ���� ����
                    userGameData.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.LogError($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. {callback}");
            }
        });
    }

    /// <summary>
    /// �ڳ� �ܼ� ���̺� �ִ� ���� ������ ����
    /// </summary>
    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("�������� �ٿ�ε� �ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." + "Insert Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param()
        {
            { "level", userGameData.level },
            { "experience", userGameData.experience },
            { "gold", userGameData.gold },
            { "jewel", userGameData.jewel},
            { "heart", userGameData.heart },
            { "dailyBestScore", userGameData.dailyBestScore }
        };

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.LogError("������ inDate ������ ���� ���� ���� ������ ������ �����߽��ϴ�.");
        }
        // ���� ������ �������� ������ ���̺� ����Ǿ� �ִ� inDate�÷��� ����
        // �����ϴ� ������ owner_inDate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ� UpdateV2() ȣ��
        else
        {
            Debug.Log($"{gameDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
                    action?.Invoke();
                    onGameDataLoadEvent.Invoke();
                }
                else
                {
                    Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. {callback}");
                }
            });
        }
    }
}
