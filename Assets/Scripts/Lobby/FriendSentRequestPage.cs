using TMPro;
using UnityEngine;

public class FriendSentRequestPage : FriendPageBase
{
    [Header("Send Request Friends")]
    [SerializeField] private TMP_InputField inputFieldNickname;
    [SerializeField] private FadeEffect_TMP textResult;

	private void OnEnable()
	{
        // [친구 요청 대기] 목록 불러오기
        backendFriendSystem.GetSentRequestList();
	}

	private void OnDisable()
	{
        DeactivateAll();
	}

	public void OnClickRequestFriend()
    {
        string nickname = inputFieldNickname.text;

        if (nickname.Trim().Equals(""))
        {
            textResult.FadeOut("친구 요청을 보낼 닉네임을 입력해주세요.");
            return;
        }

        inputFieldNickname.text = "";

        backendFriendSystem.SendRequestFriend(nickname);
    }
}
