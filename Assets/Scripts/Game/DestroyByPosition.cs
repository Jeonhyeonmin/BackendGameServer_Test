using UnityEngine;

public class DestroyByPosition : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    private float destroyWeight = 2;

	private void LateUpdate()
	{
		if (transform.position.y < stageData.LimitMin.y - destroyWeight || transform.position.y > stageData.LimitMax.y + destroyWeight ||
			transform.position.x < stageData.LimitMin.x - destroyWeight || transform.position.x > stageData.LimitMax.x + destroyWeight)
		{
			Destroy(gameObject);
		}
	}
}
