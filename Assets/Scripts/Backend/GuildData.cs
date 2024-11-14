using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GuildData : MonoBehaviour
{
    public string guildName;    // ��� �̸�
    public string guildInDate;  // ��� InDate
    public string notice;   // ��� �������� (��Ÿ ������)
    public int memberCount; // ��� �ο���
    public GuildMemberData master;  // ��� ������
    public List<GuildMemberData> viceMasterList;    // �� ��� ������ ���
}
