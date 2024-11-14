using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackRate = 0.1f;
    private float lastAttackTime = 0;

    public void WeaponAction()
    {
        // ������ �������κ��� attackrate �ð���ŭ ������ ���� ����

        if (Time.time - lastAttackTime > attackRate)
        {
            // ���� �÷��̾� ��ġ(transform.position)�� �߻�ü ������Ʈ�� ����
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            lastAttackTime = Time.time;
        }
    }
}
