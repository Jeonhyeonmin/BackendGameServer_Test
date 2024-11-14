using UnityEngine;
using TMPro;
using System.Collections;

public class FadeEffect_TMP : MonoBehaviour
{
    [SerializeField] private float effectTime = 1.5f;
    private TextMeshProUGUI effectText;

	private void Awake()
	{
		effectText = GetComponent<TextMeshProUGUI>();

		Color color = effectText.color;
		color.a = 0;
		effectText.color = color;
	}

	public void FadeOut(string text)
	{
		effectText.text = text;

		StartCoroutine(OnFade(1, 0));
	}

	private IEnumerator OnFade(float start, float end)
	{
		float current = 0;
		float percent = 0;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / effectTime;

			Color color = effectText.color;
			color.a = Mathf.Lerp(start, end, percent);
			effectText.color = color;

			yield return null;
		}
	}
}
