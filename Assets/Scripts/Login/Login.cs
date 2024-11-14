using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Login : LoginBase
{
    [SerializeField] private Image imageID;
    [SerializeField] private TMP_InputField inputFieldID;
	[SerializeField] private Image imagePW;
	[SerializeField] private TMP_InputField inputFieldPW;

	[SerializeField] private Button btnLogin; // 로그인 버튼

	/// <summary>
	/// "로그인" 버튼을 눌렀을 때 호출
	/// </summary>
	public void OnClickLogin()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageID, imagePW);

		if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

		// 로그인 버튼을 연타하지 못하도록 상호작용 비활성화
		btnLogin.interactable = false;

		// 서버에 로그인 요청하는 동안 화면에 출력하는 내용 업데이트
		// ex) 로그인 관련 텍스트 출력, 톱니바퀴 아이콘 회전 등
		StartCoroutine(nameof(LoginProcess));

		// 뛰끝 서버 로그인 시도
		ResponseToLogin(inputFieldID.text, inputFieldPW.text);
	}

	/// <summary>
	/// 로그인 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="PW"></param>
	private void ResponseToLogin(string ID, string PW)
	{
		// 서버에 로그인 요청
		Backend.BMember.CustomLogin(ID, PW, callback =>
		{
			StopCoroutine(nameof(LoginProcess));

			// 로그인 성공
			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldID.text}님 환영합니다.");

				// 모든 차트 데이터 불러오기
				BackendChartData.LoadAllChart();

				Utils.LoadScene(SceneNames.Lobby);
			}
			else
			{
				// 로그인에 실패했을 때는 다시 로그인을 해야하기 때문에 "로그인" 버튼 상호작용 활성화
				btnLogin.interactable = true;

				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 401:   // 존재하지 않는 아이디, 잘못 된 비밀번호
						message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다," : "잘못 된 비밀번호 입니다.";
						Debug.Log(callback.GetMessage());
						break;
					case 403:   // 유저 or 디바이스 차단
						message = callback.GetMessage().Contains("user") ? "차단 당한 유저입니다." : "차단 당한 디바이스 입니다.";						
						break;
					case 410:   // 탈퇴 진행 중
						message = "탈퇴가 진행 중인 유저입니다.";
						break;
					default:
						message = callback.GetMessage();
						break;
				}

				// SwatusCode 401에서 "잘못 된 비밀번호 입니다." 일 때
				if (message.Contains("비밀번호"))
				{
					GuideForIncrrectlyEnteredData(imagePW, message);
				}
				else
				{
					GuideForIncrrectlyEnteredData(imageID, message);
				}
			}
		});
	}

	private IEnumerator LoginProcess()
	{
		float time = 0;

		while (true)
		{
			time += Time.deltaTime;

			SetMessage($"로그인 중입니다... {time:F1}");

			yield return null;
		}
	}
}
