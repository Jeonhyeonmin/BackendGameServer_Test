public class FriendData
{
    public string nickname; //닉네임
    public string inDate;   // 해당 유저의 inDate
    public string createdAt;    // 친구 요청 보낸 시간 / 친구 요청 받은 시간 / 친구가 된 시간
	public string lastLogin;

	// level과 같이 출력하고 싶은 유저정보가 있으면 추가
	public string level;	// 해당 유저의 level

	public override string ToString()
	{
		string result = string.Empty;
		result += $"닉네임 : {nickname}\n";
		result += $"inDate : {inDate}\n";
		result += $"createdAt {createdAt}\n";
		result += $"lastLogin {lastLogin}\n";
		result += $"level {level}\n";

		return result;
	}
}
