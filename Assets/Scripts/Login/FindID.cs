using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindID : LoginBase
{
    [SerializeField] private Image imageEmail;  // E-mail 필드 색상 변경
    [SerializeField] private TMP_InputField inputFieldEmail;  // E-mail 필드 텍스트 정보 추출
    [SerializeField] private Button btnFindID;  // 아이디 찾기 버튼 (상호작용 가능 / 불가능)

    public void OnClickFindID()
    {
        // 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
        ResetUI(imageEmail);

        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncrrectlyEnteredData(imageEmail, "메일 형식이 잘못되었습니다. (ex. address@xx.xx");
            return;
        }

        // "아이디 찾기" 버튼의 상호작용 비활성화
        btnFindID.interactable = false;
        SetMessage("메일 발송 중입니다...");

        FindCustomID();
    }

    private void FindCustomID()
    {
        // 아이디 정보를 이메일로 전송
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            // "아이디 찾기" 버튼 상호작용 활성화
            btnFindID.interactable = true;

            // 메일 발송 성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text} 주소로 메일을 발송했습니다.");
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404:   // 해당 이메일의 게이머가 없는 경우
                        message = "해당 이메일을 사용하는 사용자가 없습니다.";
                        break;
                    case 429:   // 24시간 이내에 5회 이상 같은 이메일 정보로 아이디 / 비밀번호 찾기를 시도한 경우
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
