using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMessage;

    /// <summary>
    /// �޽��� ����, InputField ���� �ʱ�ȭ
    /// </summary>
    protected void ResetUI(params Image[] images)
    {
        textMessage.text = string.Empty;

        for (int i =0; i <images.Length; ++i)
        {
            images[i].color = Color.white;
        }
    }    

    /// <summary>
    /// �Ű����� �ִ� ������ ���
    /// </summary>
    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
    }

    /// <summary>
    /// �Է� ������ �ִ� InputField�� ���� ����
    /// ������ ���� �޽��� ���
    /// </summary>
    protected void GuideForIncrrectlyEnteredData(Image image, string msg)
    {
        textMessage.text = msg;
        image.color = Color.red;
    }

    /// <summary>
    /// �ʵ� ���� ����ִ��� Ȯ�� (image : �ʵ�, field : ����, result : ��� �Ǵ� ����)
    /// </summary>
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncrrectlyEnteredData(image, $"\"{result}\" �ʵ带 ä���ּ���.");

            return true;
        }

        return false;
    }
}
