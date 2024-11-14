using UnityEngine;

public class FriendReceivedRequestPage : FriendPageBase
{
	private void OnEnable()
	{
		// [친구 수락 대기] 목록 불러오기
		backendFriendSystem.GetReceiveRequestList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}
}
