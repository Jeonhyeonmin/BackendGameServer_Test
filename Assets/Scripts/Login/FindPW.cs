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
		// �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
		ResetUI(imageID, imageEmail);

		if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncrrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�. (Exception.address@xx.xx)");
			return;
		}

		btnFindPW.interactable = false;
		SetMessage("���� �߼� ���Դϴ�...");

		// �ڳ� ���� ��й�ȣ ã�� �õ�
		FindCustomPW();
	}

	private void FindCustomPW()
	{
		// ���� �� ��й�ȣ ������ �̸��Ϸ� �߼�
		Backend.BMember.ResetPassword(inputFieldID.text, inputFieldEmail.text, callback =>
		{
			btnFindPW.interactable = true;

			if (callback.IsSuccess())
			{
				SetMessage($"{inputFieldEmail.text} �ּҷ� ������ �߼��Ͽ����ϴ�.");
			}
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 404:   // �ش� �̸����� ���̸Ӱ� ���� ���
						message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�.";
						break;
					case 429:
						message = "24�ð� �̳��� 5ȸ �̻� ���̵� / ��й�ȣ ã�⸦ �õ��߽��ϴ�.";
						break;
					default:
						// statusCode : 400 => ������Ʈ�� Ư�����ڰ� �߰��� ��� (�ȳ� ���� �̹߼� �� ���� �߻�)
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("�̸���"))
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
