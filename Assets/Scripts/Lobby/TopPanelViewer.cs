using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private Slider sliderExperience;
    [SerializeField] private TextMeshProUGUI textHeart;
    [SerializeField] private TextMeshProUGUI textJewel;
    [SerializeField] private TextMeshProUGUI textGold;

	private void Awake()
	{
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdategameData);
	}

	public void UpdateNickname()
    {
        // �г����� ������ gamer_id�� ����ϰ�, �г����� ������ �г��� ���
        textNickname.text = UserInfo.Data.nickname ?? UserInfo.Data.gamerId;
    }

    public void UpdategameData()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        textLevel.text = $"{currentLevel.ToString()}";
        // �ӽ÷� �ִ� ����ġ�� 100���� ����
        sliderExperience.value = BackendGameData.Instance.UserGameData.experience / BackendChartData.levelChart[currentLevel - 1].maxExperience;
        textHeart.text = $"{BackendGameData.Instance.UserGameData.heart} / 30";
        textJewel.text = $"{BackendGameData.Instance.UserGameData.jewel}";
        textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
    }
}
