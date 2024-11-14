using UnityEngine;

public class FriendReceivedRequestPage : FriendPageBase
{
	private void OnEnable()
	{
		// [ģ�� ���� ���] ��� �ҷ�����
		backendFriendSystem.GetReceiveRequestList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}
}
