using System.Collections;
using TMPro;
using UnityEngine;

public class CountingEffect : MonoBehaviour
{
    [SerializeField][Range(0.01f, 10f)] private float effectTime;
    private TextMeshProUGUI effectText;

	private void Awake()
	{
		effectText = GetComponent<TextMeshProUGUI>();
	}

	public void Play(int start, int end)
	{
		StartCoroutine(Process(start, end));
	}

	private IEnumerator Process(int start, int end)
	{
		float current = 0;
		float percent = 0;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / effectTime;

			effectText.text = Mathf.Lerp(start, end, percent).ToString("F0");

			yield return null;
		}
	}
}
