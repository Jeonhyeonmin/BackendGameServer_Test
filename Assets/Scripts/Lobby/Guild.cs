using TMPro;
using UnityEngine;

public class Guild : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGuildName;
    [SerializeField] private TextMeshProUGUI textMasterNickname;
    [SerializeField] private TextMeshProUGUI textMemberCount;

    public void Setup(GuildData guildData)
    {
        textGuildName.text = guildData.guildName;
        textMasterNickname.text = guildData.master.nickname;
        textMemberCount.text = $"{guildData.memberCount} / 100";
    }
}
