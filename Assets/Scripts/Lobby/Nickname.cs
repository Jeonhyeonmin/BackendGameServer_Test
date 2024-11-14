using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEvent { }
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField] private Image imageNickname;
    [SerializeField] private TMP_InputField inputFieldNickname;
    [SerializeField] private Button btnUpdateNickname;

    private void OnEnable()
    {
        // �г��� ���濡 ������ ���� �޽����� ����� ���¿���
        // �г��� ���� �˾��� �ݾҴٰ� �� �� �ֱ� ������ ���¸� �ʱ�ȭ
        ResetUI(imageNickname);
        SetMessage("�г����� �Է��ϼ���.");
    }

    public void OnClickUpdateNickname()
    {
        // �Ű������� �Է��� InputField UI�� ����� message ���� �ʱ�ȭ
        ResetUI(imageNickname);

        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        btnUpdateNickname.interactable = false;
        SetMessage("�г��� ���� ���Դϴ�.");

        // �ڳ� ���� �г��� ���� �õ�
        UpdateNickname();
    }

    private void UpdateNickname()
    {
        // �г��� ����
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            // �г��� ���� ��ư Ȱ��ȭ
            btnUpdateNickname.interactable = true;

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(��)�� �г����� ����Ǿ����ϴ�.");

                // �г��� ���濡 �������� �� onNicknameEvent�� ��ϵǾ� �ִ� �̺�Ʈ �ż��� ȣ��
                onNicknameEvent.Invoke();
            }
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400:   // �� �г��� Ȥ�� string.Empty 20�� �̻��� �г���, �г��� �� / �� ������ �ִ� ���
                        message = "�г����� ����ų� | 20�� �̻��̰ų� | �� / �� ������ �ֽ��ϴ�.";
                        break;
                    case 409:   // �̹� �ߺ� �� �г��� �ִ� ���
                        message = "�̹� �����ϴ� �г����Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                GuideForIncrrectlyEnteredData(imageNickname, message);
            }
        });
    }
}
