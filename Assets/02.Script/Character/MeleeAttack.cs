using UnityEngine;

public class MeleeAttack : MonoBehaviour, IEnemyAttack
{
    [Header("Ref")]
    [SerializeField] private CharacterStats stats;
    // 공격력 등 스탯 정보 가져오기

    [SerializeField] private Transform attackPoint;
    // 실제 공격 판정이 발생할 위치 (앞쪽)

    [Header("Attack")]
    [SerializeField] private LayerMask targetLayer;
    // 공격 대상 레이어 (플레이어 등)

    [SerializeField] private Vector2 boxSize = new Vector2(2f, 1f);
    // 공격 범위 (가로, 세로 크기)

    [Header("방향")]
    [SerializeField] private bool isFacingRight = false;
    // 몬스터가 오른쪽을 보고 있는지 여부
   

    [SerializeField] private float attackPointX = 1f;
    // 몬스터 중심에서 공격 판정까지 거리

 
    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
        // 방향 값 저장

        UpdateAttackPointPosition();
        // 방향에 맞게 공격 위치 갱신
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null)
        {
            return;
        }

        // 현재 로컬 위치 가져오기
        Vector3 localPos = attackPoint.localPosition;

        if (isFacingRight)
        {
            // 오른쪽을 보면 +값
            localPos.x = attackPointX;
        }
        else
        {
            // 왼쪽을 보면 -값
            localPos.x = -attackPointX;
        }

        // 위치 적용
        attackPoint.localPosition = localPos;
    }

 
    public void ExecuteAttack()
    {
        // 스탯 없으면 가져오기
        if (stats == null)
        {
            stats = GetComponent<CharacterStats>();
        }

        // 공격 위치 없으면 중단
        if (attackPoint == null)
        {
            return;
        }

        // 공격하기 전에 방향에 맞게 위치 업데이트
        UpdateAttackPointPosition();

        // OverlapBox로 박스 범위 안에 있는 대상 1개 감지
        Collider2D hit = Physics2D.OverlapBox(
            attackPoint.position, // 공격 중심 위치
            boxSize,              // 범위 크기
            0f,                   // 회전 없음
            targetLayer           // 대상 레이어
        );

        // 맞은 대상이 있으면
        if (hit != null)
        {
            // IDamageable인지 확인
            IDamageable damageable = hit.GetComponent<IDamageable>();

            Debug.Log("플레이어 맞음: " + hit.name);

            if (damageable != null)
            {
                // 공격력 가져오기
                int damage = stats.GetAttackDamage();

                // 데미지 적용
                damageable.TakeDamage(damage, gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        // 방향 반영해서 위치 맞추기
        UpdateAttackPointPosition();

        Gizmos.color = Color.red;

        // 박스 형태로 공격 범위 표시
        Gizmos.DrawWireCube(attackPoint.position, boxSize);
    }
}