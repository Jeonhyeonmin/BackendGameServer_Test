public class FriendPage : FriendPageBase
{
	private void OnEnable()
	{
		// [친구] 목록 불러오기
		backendFriendSystem.GetFriendList();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}
}
