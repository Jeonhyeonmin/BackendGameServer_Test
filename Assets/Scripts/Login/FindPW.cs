using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindPW : LoginBase
{
    [SerializeField] private Image imageID;
    [SerializeField] private TMP_InputField inputFieldID;
	[SerializeField] private Image imageEmail;
	[SerializeField] private TMP_InputField inputFieldEmail;

	[SerializeField] private Button btnFindPW;

	public void OnClickFindPW()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageID, imageEmail);

		if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncrrectlyEnteredData(imageEmail, "메일 형식이 잘못되었습니다. (Exception.address@xx.xx)");
			return;
		}

		btnFindPW.interactable = false;
		SetMessage("메일 발송 중입니다...");

		// 뒤끝 서버 비밀번호 찾기 시도
		FindCustomPW();
	}

	private void FindCustomPW()
	{
		// 리셋 된 비밀번호 정보를 이메일로 발송
		Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
		{
			btnFindPW.interactable = true;

			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldEmail.text} 주소로 메일을 발송하였습니다.");
			}
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 404:   // 해당 이메일의 게이머가 없는 경우
						message = "해당 이메일을 사용하는 사용자가 없습니다.";
						break;
					case 429:
						message = "24시간 이내에 5회 이상 아이디 / 비밀번호 찾기를 시도했습니다.";
						break;
					default:
						// statusCode : 400 => 프로젝트명에 특수문자가 추가된 경우 (안내 메일 미발송 및 에러 발생)
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("이메일"))
				{
					GuideForIncrrectlyEnteredData(imageEmail, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}
