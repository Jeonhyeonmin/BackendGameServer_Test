using System.Collections.Generic;
using UnityEngine;

public class PostData : MonoBehaviour
{
    public string title;
    public string content;
    public string inDate;
    public string expirationDate;

    public bool isCanReceive = false;   // 우편에 받을 수 있는 아이템이 있는지 여부

    public Dictionary<string, int> postReward = new Dictionary<string, int>();

	public override string ToString()
	{
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "우편아이템\n";

            foreach (string itemKey in postReward.Keys)
            {
                result += $"| {itemKey} : {postReward[itemKey]}개\n";
            }
        }
        else
        {
            result += "지원하지 않는 아이템입니다.";
        }

        return result;
	}
}
