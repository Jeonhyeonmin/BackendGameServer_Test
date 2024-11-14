using UnityEngine;

public class GuildApplicantsPage : MonoBehaviour
{
    [SerializeField] private BackendGuildSystem backendGuildSystem;
    [SerializeField] private GameObject applicantPrefab;
    [SerializeField] private Transform parentContent;
    [SerializeField] private GameObject textSystem; // 해당 페이지가 비어있을 때 출력하는 Text UI

    private MemoryPool memoryPool;

	private void Awake()
	{
        memoryPool = new MemoryPool(applicantPrefab, parentContent);
	}

	private void OnEnable()
	{
		backendGuildSystem.GetApplicants();
	}

	private void OnDisable()
	{
		DeactivateAll();
	}

	public void Activate(GuildMemberData applicant)
	{
        if (textSystem.activeSelf)
        {
			textSystem.SetActive(false);
        }

		GameObject item = memoryPool.ActivePoolItem();
		item.GetComponent<GuildApplicant>().Setup(backendGuildSystem, this, applicant);
    }

	public void Deactivate(GameObject applicant)
	{
		memoryPool.DeactivatepoolItem(applicant);

		if (memoryPool.ActiveCount == 0)
		{
			textSystem.SetActive(true);
		}
	}

	public void DeactivateAll()
	{
		textSystem.SetActive(true);
		memoryPool.DeactivateAllPoolItems();
	}
}
