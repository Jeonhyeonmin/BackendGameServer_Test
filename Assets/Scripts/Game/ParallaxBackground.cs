using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform target;  // 현재 배경과 이어지는 배경
    [SerializeField] private float scrollAmount = 14.08f;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private Vector3 moveDirection = Vector3.down;

	private void Update()
	{
		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		if (transform.position.y <= -scrollAmount)
		{
			transform.position = target.position - moveDirection * scrollAmount;
		}
	}
}
