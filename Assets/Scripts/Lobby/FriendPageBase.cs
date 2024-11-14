using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FriendPageBase : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected BackendFriendSystem backendFriendSystem;

    [Header("Friend Page Base")]
    [SerializeField] private GameObject friendPrefab;   // 해당 페이지에 출력하는 개별 친구 UI

    [SerializeField] private Transform parentContent;   // UI가 배치되는 ScrollView의 Content
    [SerializeField] private GameObject textSystem; // 해당 페이지가 비어있을 때 출력하는 Text UI

    private MemoryPool memoryPool;

	private void Awake()
	{
        memoryPool = new MemoryPool(friendPrefab, parentContent);
	}

    public void Activate(FriendData friend)
    {
        if (textSystem.activeSelf)
        {
            textSystem.SetActive(false);
        }

		GameObject item = memoryPool.ActivePoolItem();
        item.GetComponent<FriendBase>().Setup(backendFriendSystem, this, friend);
	}

    public void ActivateAll(List<FriendData> friendList)
    {
        for (int i = 0; i < friendList.Count; ++i)
        {
            Activate(friendList[i]);
        }
    }

    public void DeactivateAll()
    {
        textSystem.SetActive(true);

        memoryPool.DeactivateAllPoolItems();
    }

    public void Deactivate(GameObject friend)
    {
        memoryPool.DeactivatepoolItem(friend);

        if (memoryPool.ActiveCount == 0)
        {
            textSystem.SetActive(true);
        }
    }
}
