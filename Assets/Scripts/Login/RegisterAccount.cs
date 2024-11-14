using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterAccount : LoginBase
{
    [SerializeField] private Image imageID;
    [SerializeField] private TMP_InputField inputFieldID;
	[SerializeField] private Image imagePW;
	[SerializeField] private TMP_InputField inputFieldPW;
	[SerializeField] private Image imageConfirmPW;
	[SerializeField] private TMP_InputField inputFieldConfirmPW;
	[SerializeField] private Image imageEmail;
	[SerializeField] private TMP_InputField inputFieldEmail;

	[SerializeField] private Button btnRegisterAccount; // "계정 생성" 버튼

	/// <summary>
	/// 계정 생성 버튼을 눌렀을 때
	/// </summary>
	public void OnClickRegisterAccount()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

		if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;
		if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "비밀번호 확인")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

		// 비밀번호와 비밀번호 확인의 내용이 다를 때
		if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
		{
			GuideForIncrrectlyEnteredData(imageConfirmPW, "비밀번호가 일치하지 않습니다.");
			return;
		}

		// 메일 형식 검사
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncrrectlyEnteredData(imageEmail, "메일 형식이 잘못되었습니다. (e. address@xx.xx)");
			return;
		}

		// 계정 생성 버튼 상호작용 비활성화
		btnRegisterAccount.interactable = false;
		SetMessage("계정 생성 중입니다...");

		// 뒤끝 서버 계정 생성 시도
		CustomSignUp();
	}

	/// <summary>
	/// 계정 생성 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
	/// </summary>
	private void CustomSignUp()
	{
		Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
		{
			// "계정 생성" 버튼 상호작용 활성화
			btnRegisterAccount.interactable = true;

			if (callback.IsSuccess())
			{
				// E-mail 정보 업데이트 - 계정 생성 시 해당 계정으로 접속한 상태가 되므로 이때, 이메일 설정 시 계정의 이메일이 된다.
				Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
				{
					if (callback.IsSuccess())
					{
						SetMessage($"계정 생성 성공, {inputFieldID.text}님 환영합니다.");

						// 계정 생성에 성공했을 때, 해당 계정의 게임 정보 생성
						BackendGameData.Instance.GameDataInsert();

						// 모든 차트 데이터 불러오기
						BackendChartData.LoadAllChart();

						// Lobby 씬으로 이동
						Utils.LoadScene(SceneNames.Lobby);
					}
				});
			}
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 409:	// 중복된  customID가 존재하는 경우
						message = "이미 존재하는 아이디입니다.";
						break;
					case 403:   // 차단당한 디바이스인 경우
					case 401:	// 프로젝트 상테가 '점검'인 경우
					case 400:   // 디바이스 정보가 null일 경우
					default:
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("아이디"))
				{
					GuideForIncrrectlyEnteredData(imageID, message);
				}
				else
				{
					SetMessage(message);
				}
			}
		});
	}
}
