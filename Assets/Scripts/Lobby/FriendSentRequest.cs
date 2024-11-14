public class FriendSentRequest : FriendBase
{
	public override void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		base.Setup(friendSystem, friendPage, friendData);
		base.SetExpirationDate();
	}

	public void OnClickCancelRequest()
	{
		// 친구 UI 오브젝트 비활성화
		friendPageBase.Deactivate(gameObject);
		backendFriendSystem.RevokeSentRequest(friendData.inDate);
	}
}
