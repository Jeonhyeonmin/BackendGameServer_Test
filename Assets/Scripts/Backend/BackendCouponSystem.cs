using BackEnd;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BackendCouponSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldCode;
    [SerializeField] private FadeEffect_TMP textResult;

	public void ReceiveCoupon()
    {
        string couponCode = inputFieldCode.text;

        if (couponCode.Trim().Equals(""))
        {
            textResult.FadeOut("쿠폰 코드를 입력해주세요.");
            return;
        }

        inputFieldCode.text = "";

        ReceiveCoupon(couponCode);
    }

    public void ReceiveCoupon(string couponCode)
    {
        Backend.Coupon.UseCoupon(couponCode, callback =>
        {
            if (!callback.IsSuccess())
            {
                // 쿠폰 받기에 실패했을 때 처리
                FailedToReceive(callback);
                return;
            }

            // JSON 데이터 파싱 성공
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["itemObject"];
				LitJson.JsonData jsonData1 = callback.GetReturnValuetoJSON();
                Debug.Log(jsonData1);

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("쿠폰에 아이템이 없습니다.");
                }

                // 쿠폰에 있는 모든 아이템 저장
                SaveToLocal(jsonData);
            }
            // JSON 데이터 파싱 실패
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        });
    }

    private void FailedToReceive(BackendReturnObject callback)
    {
        if (callback.GetMessage().Contains("전부 사용된"))
        {
            textResult.FadeOut("쿠폰 발생 개수가 소진되었거나 기간이 만료된 쿠폰입니다.");
        }    
        else if (callback.GetMessage().Contains("이미 사용하신 쿠폰"))
        {
            textResult.FadeOut("해당 쿠폰은 이미 사용하셨습니다.");

        }
        else
        {
            textResult.FadeOut("쿠폰 코드가 잘못되었거나 이미 사용한 쿠폰입니다.");
        }

        Debug.LogError($"쿠폰 사용 중 에러가 발생했습니다 : {callback}");
    }

    private void SaveToLocal(LitJson.JsonData items)
    {
        // JSON 데이터 파싱 성공
        try
        {
            string getItems = string.Empty;

            foreach (LitJson.JsonData item in items)
            {
                int itemId = int.Parse(item["item"]["itemId"].ToString());
                string itemName = item["item"]["itemName"].ToString();
                string itemInfo = item["item"]["itemInfo"].ToString();
                int itemCount = int.Parse(item["itemCount"].ToString());

                if (itemName.Equals("heart"))
                {
                    BackendGameData.Instance.UserGameData.heart += itemCount;
                }
                else if (itemName.Equals("gold"))
                {
                    BackendGameData.Instance.UserGameData.gold += itemCount;
                }
                else if (itemName.Equals("jewel"))
                {
                    BackendGameData.Instance.UserGameData.jewel += itemCount;
                }

                getItems += $"[{itemName}:{itemCount}]";
            }

            textResult.FadeOut($"쿠폰 사용으로 아이템 {getItems}을 획득했습니다.");

            BackendGameData.Instance.GameDataUpdate();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
