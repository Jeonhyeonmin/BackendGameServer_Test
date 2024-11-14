public class FriendReceivedRequest : FriendBase
{
	public override void Setup(BackendFriendSystem friendSystem, FriendPageBase friendPage, FriendData friendData)
	{
		base.Setup(friendSystem, friendPage, friendData);
		base.SetExpirationDate();
	}

	public void OnClickAcceptRequest()
	{
		// 친구 UI 오브젝트 삭제
		friendPageBase.Deactivate(gameObject);

		// 친구 요청 수락 (Backend Console)
		backendFriendSystem.AcceptFriend(friendData);
	}

	public void OnClickRejectRequest()
	{
		// 친구 UI 오브젝트 삭제
		friendPageBase.Deactivate(gameObject);
		backendFriendSystem.RejectFriend(friendData);
	}
}
