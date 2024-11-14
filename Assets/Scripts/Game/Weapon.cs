using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackRate = 0.1f;
    private float lastAttackTime = 0;

    public void WeaponAction()
    {
        // 마지막 공격으로부터 attackrate 시간만큼 지나야 공격 가능

        if (Time.time - lastAttackTime > attackRate)
        {
            // 현재 플레이어 위치(transform.position)에 발사체 오브젝트를 생성
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            lastAttackTime = Time.time;
        }
    }
}
