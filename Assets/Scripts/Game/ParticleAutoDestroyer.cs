using UnityEngine;

public class ParticleAutoDestroyer : MonoBehaviour
{
    private ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		// 파티클 재생이 완료되면 삭제
		if (particle.isPlaying == false)
		{
			Destroy(gameObject);
		}
	}
}
