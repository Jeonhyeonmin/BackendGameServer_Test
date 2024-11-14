using System.Collections.Generic;
using UnityEngine;

public class PostData : MonoBehaviour
{
    public string title;
    public string content;
    public string inDate;
    public string expirationDate;

    public bool isCanReceive = false;   // ���� ���� �� �ִ� �������� �ִ��� ����

    public Dictionary<string, int> postReward = new Dictionary<string, int>();

	public override string ToString()
	{
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "���������\n";

            foreach (string itemKey in postReward.Keys)
            {
                result += $"| {itemKey} : {postReward[itemKey]}��\n";
            }
        }
        else
        {
            result += "�������� �ʴ� �������Դϴ�.";
        }

        return result;
	}
}
