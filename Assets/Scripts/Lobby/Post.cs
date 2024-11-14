using BackEnd;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Post : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteItemIcons;  // ���� ���Ե� ������ �����ܿ� ����� �̹��� �迭
    [SerializeField] private Image imageItemIcon;   // ���� ���� �� ������ ������ ���
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private TextMeshProUGUI textExpirationDate;

    [SerializeField] private Button buttonReceive;  // ���� "����" ��ư ó��

    private BackendPostSystem backendPostSystem;
    private PopupPostBox popupPostBox;
    private PostData postData;

    public void Setup(BackendPostSystem postSystem, PopupPostBox popupBox ,PostData postData)
    {
        // ���� "����" ��ư�� ������ �� ó��
        buttonReceive.onClick.AddListener(OnClickPostReceive);
        backendPostSystem = postSystem;
		popupPostBox = popupBox;
        this.postData = postData;

        textTitle.text = postData.title;
        textContent.text = postData.content;

        // ù ��° ������ ������ ���� ���
        foreach (string itemkey in postData.postReward.Keys)
        {
            // ���� ���Ե� ������ �̹��� ���
            if (itemkey.Equals("heart"))
            {
                imageItemIcon.sprite = spriteItemIcons[0];
            }
            else if (itemkey.Equals("gold"))
            {
                imageItemIcon.sprite = spriteItemIcons[1];
            }
            else if (itemkey.Equals("jewel"))
            {
                imageItemIcon.sprite = spriteItemIcons[2];
            }

            textItemCount.text = postData.postReward[itemkey].ToString();

            // �ϳ��� ���� ���Ե� �������� ���� �� �� ���� �ִµ� ���� ���������� ù ��° ������ ������ ���
            break;
        }

        // GetServerTime()
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. {callback}");
                return;
            }

            //JSON ������ �Ľ� ����
            try
            {
                // ���� ���� �ð�
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();

                // ���� ������� ���� �ð� = ���� ����ð� - ���� ���� �ð�
                TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);

                // timeSpan.TotalHours�� ���� �ð��� ��(hour)�� ����
                textExpirationDate.text = $"{timeSpan.TotalHours:F0}�ð� �� ����";
            }
            catch (Exception e)
            {
                // try-catch�� ���� ���
                Debug.LogError(e);
            }
        });
    }

    private void OnClickPostReceive()
    {
        // ���� ���� UI ������Ʈ ����
        popupPostBox.DestroyPost(gameObject);
        backendPostSystem.PostReceive(PostType.Admin, postData.inDate);
    }
}
