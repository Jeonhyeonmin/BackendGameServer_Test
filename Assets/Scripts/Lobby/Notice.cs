using TMPro;
using UnityEngine;

public class Notice : MonoBehaviour
{
    [SerializeField] private BackendGuildSystem backendGuildSystem;
    [SerializeField] private GameObject noticebackground;   // 길드원에게 보이는 오브젝트
    [SerializeField] private TextMeshProUGUI textNotice;    // 길드원에게 보이는 공지사항
    [SerializeField] private TMP_InputField inputFieldNotice;
    [SerializeField] private FadeEffect_TMP textLog;

    public void Setup(bool isMaster = false, bool isOtherGuild = false)
    {
        if (isOtherGuild == true)
        {
			textNotice.text = backendGuildSystem.otherGuildDate.notice;
			inputFieldNotice.text = backendGuildSystem.otherGuildDate.notice;
		}
        else
        {
			textNotice.text = backendGuildSystem.myGuildData.notice;
			inputFieldNotice.text = backendGuildSystem.myGuildData.notice;
		}
        

        noticebackground.SetActive(!isMaster);
        inputFieldNotice.gameObject.SetActive(isMaster);
    }

    public void OnClickNoticeRegisteration()
    {
        string notice = inputFieldNotice.text;

        if (notice.Trim().Equals(""))
        {
            textLog.FadeOut("공지사항 내용이 비어있습니다.");
            return;
        }

        backendGuildSystem.SetNotice(notice);
    }
}
