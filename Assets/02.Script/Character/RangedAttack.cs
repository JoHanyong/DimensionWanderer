using UnityEngine;

public class RangedAttack : MonoBehaviour, IEnemyAttack
{
    [Header("Ref")]
    [SerializeField] private CharacterStats stats; //공격력 + 사거리
    [SerializeField] private Transform firePoint; //탈환 발사 위치

    [Header("Projectile")]
    [SerializeField] private ProjectileBullet projectilePrefab; //생성할 탄환 프리팹
    [SerializeField] private LayerMask targetLayer;

    public void ExecuteAttack()
    {
        if(stats == null)
        {
            stats = GetComponent<CharacterStats>();
        }
        if(projectilePrefab == null || firePoint == null)
        {
            return;
        }
       
        ProjectileBullet bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); //탄환 생성

        Vector2 direction;

        if (transform.localScale.x >= 0)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        bullet.Initializee(stats, direction, targetLayer, stats.AttackRange);

    }
}
