using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    // 스테이지 크기 (상하좌우 범위)를 limitMin, limitMax 변수에 저장
    [SerializeField] private Vector2 limitMin;
    [SerializeField] private Vector2 limitMax;

    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
