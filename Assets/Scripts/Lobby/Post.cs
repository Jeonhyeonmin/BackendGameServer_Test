using BackEnd;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Post : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteItemIcons;  // 우편에 포함된 아이템 아이콘에 출력할 이미지 배열
    [SerializeField] private Image imageItemIcon;   // 우편에 포함 된 아이템 아이콘 출력
    [SerializeField] private TextMeshProUGUI textItemCount;
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private TextMeshProUGUI textExpirationDate;

    [SerializeField] private Button buttonReceive;  // 우편 "수령" 버튼 처리

    private BackendPostSystem backendPostSystem;
    private PopupPostBox popupPostBox;
    private PostData postData;

    public void Setup(BackendPostSystem postSystem, PopupPostBox popupBox ,PostData postData)
    {
        // 우편 "수령" 버튼을 눌렀을 때 처리
        buttonReceive.onClick.AddListener(OnClickPostReceive);
        backendPostSystem = postSystem;
		popupPostBox = popupBox;
        this.postData = postData;

        textTitle.text = postData.title;
        textContent.text = postData.content;

        // 첫 번째 아이템 정보를 우편에 출력
        foreach (string itemkey in postData.postReward.Keys)
        {
            // 우편에 포함된 아이템 이미지 출력
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

            // 하나의 우편에 포함된 아이템이 여러 개 일 수도 있는데 현재 예제에서는 첫 번째 아이템 정보만 출력
            break;
        }

        // GetServerTime()
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"서버 시간 불러오기에 실패했습니다. {callback}");
                return;
            }

            //JSON 데이터 파싱 성공
            try
            {
                // 현재 서버 시간
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();

                // 우편 만료까지 남은 시간 = 우편 만료시간 - 현재 서버 시간
                TimeSpan timeSpan = DateTime.Parse(postData.expirationDate) - DateTime.Parse(serverTime);

                // timeSpan.TotalHours로 남은 시간을 시(hour)로 포현
                textExpirationDate.text = $"{timeSpan.TotalHours:F0}시간 후 만료";
            }
            catch (Exception e)
            {
                // try-catch문 에러 출력
                Debug.LogError(e);
            }
        });
    }

    private void OnClickPostReceive()
    {
        // 현재 우편 UI 오브젝트 삭제
        popupPostBox.DestroyPost(gameObject);
        backendPostSystem.PostReceive(PostType.Admin, postData.inDate);
    }
}
