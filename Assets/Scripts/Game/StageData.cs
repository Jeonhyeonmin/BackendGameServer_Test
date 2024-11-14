using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    // �������� ũ�� (�����¿� ����)�� limitMin, limitMax ������ ����
    [SerializeField] private Vector2 limitMin;
    [SerializeField] private Vector2 limitMax;

    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
