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

	[SerializeField] private Button btnRegisterAccount; // "���� ����" ��ư

	/// <summary>
	/// ���� ���� ��ư�� ������ ��
	/// </summary>
	public void OnClickRegisterAccount()
	{
		// �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
		ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

		if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
		if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
		if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
		if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

		// ��й�ȣ�� ��й�ȣ Ȯ���� ������ �ٸ� ��
		if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
		{
			GuideForIncrrectlyEnteredData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
			return;
		}

		// ���� ���� �˻�
		if (!inputFieldEmail.text.Contains("@"))
		{
			GuideForIncrrectlyEnteredData(imageEmail, "���� ������ �߸��Ǿ����ϴ�. (e. address@xx.xx)");
			return;
		}

		// ���� ���� ��ư ��ȣ�ۿ� ��Ȱ��ȭ
		btnRegisterAccount.interactable = false;
		SetMessage("���� ���� ���Դϴ�...");

		// �ڳ� ���� ���� ���� �õ�
		CustomSignUp();
	}

	/// <summary>
	/// ���� ���� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
	/// </summary>
	private void CustomSignUp()
	{
		Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
		{
			// "���� ����" ��ư ��ȣ�ۿ� Ȱ��ȭ
			btnRegisterAccount.interactable = true;

			if (callback.IsSuccess())
			{
				// E-mail ���� ������Ʈ - ���� ���� �� �ش� �������� ������ ���°� �ǹǷ� �̶�, �̸��� ���� �� ������ �̸����� �ȴ�.
				Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
				{
					if (callback.IsSuccess())
					{
						SetMessage($"���� ���� ����, {inputFieldID.text}�� ȯ���մϴ�.");

						// ���� ������ �������� ��, �ش� ������ ���� ���� ����
						BackendGameData.Instance.GameDataInsert();

						// ��� ��Ʈ ������ �ҷ�����
						BackendChartData.LoadAllChart();

						// Lobby ������ �̵�
						Utils.LoadScene(SceneNames.Lobby);
					}
				});
			}
			else
			{
				string message = string.Empty;

				switch (int.Parse(callback.GetStatusCode()))
				{
					case 409:	// �ߺ���  customID�� �����ϴ� ���
						message = "�̹� �����ϴ� ���̵��Դϴ�.";
						break;
					case 403:   // ���ܴ��� ����̽��� ���
					case 401:	// ������Ʈ ���װ� '����'�� ���
					case 400:   // ����̽� ������ null�� ���
					default:
						message = callback.GetMessage();
						break;
				}

				if (message.Contains("���̵�"))
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
