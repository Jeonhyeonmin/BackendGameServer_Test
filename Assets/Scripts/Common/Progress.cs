using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class Progress : MonoBehaviour
{
    [SerializeField] private Slider sliderProgress;
    [SerializeField] private TextMeshProUGUI textProgressData;
    [SerializeField] private float progressTime;

    public void Play(UnityAction action = null)
    {
        StartCoroutine(OnProgress(action));
    }

    private IEnumerator OnProgress(UnityAction action)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;

            // Text 정보 설정
            textProgressData.text = $"Now Loading... {sliderProgress.value * 100:F0}%";
			sliderProgress.value = Mathf.Lerp(0, 1, percent);

            yield return null;
        }

        // action이 null이 아니면 action 매소드 실행
        action?.Invoke();
    }
}
