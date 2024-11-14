using UnityEngine;

public class GuildMemberData
{
    public string nickname;
    public string inDate;
    public string level;
	public string position; // 길드 직책
	public int goodsCount;  // 굿즈 개수
	public string lastLogin;	// 마지막 접속 날짜

	public override string ToString()
	{
		string result = string.Empty;
		result += $"nickname : {nickname}\n";
		result += $"inDate : {inDate}\n";
		result += $"level : {level}\n";
		result += $"position : {position}\n";
		result += $"goodsCount : {goodsCount}\n";
		result += $"lastLogin : {lastLogin}\n";

		return base.ToString();
	}
}
