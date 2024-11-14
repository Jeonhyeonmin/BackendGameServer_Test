using TMPro;
using UnityEngine;

public class PopupUpdateProfileViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textGamerID;

    public void UpdateNickname()
    {
        // �г����� ������ gamer_id�� ����ϰ�, �г����� ������ �г��� ���
        textNickname.text = UserInfo.Data.nickname ?? UserInfo.Data.gamerId;
        textGamerID.text = UserInfo.Data.gamerId;
    }
}
