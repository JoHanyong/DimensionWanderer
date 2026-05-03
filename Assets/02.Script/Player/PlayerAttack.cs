using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerInputReader inputReader; //공격 버튼 입력받기
    [SerializeField] private PlayerMove2D movement; //방향 보기
    [SerializeField] private CharacterStats stars;
    

    [Header("Attack")]
    [SerializeField] private Transform attackPoint; //공격 판정 위치
    [SerializeField] private Vector2 attackHitBox = new Vector2(1.2f, 0.8f); // 공격 범위 나중에 조정\
    [SerializeField] private LayerMask targetLayer; //레이어 공격 대상
    [SerializeField] private float lastAttackTime = -999f; //바로 공격 가능하게

    [Header("공격모션")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if(inputReader == null)
        {
            inputReader = GetComponent<PlayerInputReader>();
        }
        if(movement == null)
        {
            movement = GetComponent<PlayerMove2D>();
        }
        if(stars == null)
        {
            stars = GetComponent<CharacterStats>();
        }
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    private void Update()
    {
        HandleAttack();
        AttackPositionDirection(); //방향갱신 
    }
    private void HandleAttack() //공격 판단 입력/쿨타임
    {
        if (!inputReader.AttackPressedThisFrame) //입력이 없으면 반환
        {
            return;
        }
        if (Time.time < lastAttackTime + stars.AttackCooldown) //쿨타임 적으면 공격불가
        {
            return ;
        }

        lastAttackTime = Time.time;

        PlayAttackAnimation();

        PerformAttack();

        
    }
    private void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("AttackSlash");
        }
    }
    private void PerformAttack() //공격 실행
    {
        SoundManager.Instance.PlayAttackSFX();
        Vector2 boxSize = new Vector2(stars.AttackRange, attackHitBox.y);

        Collider2D hit = Physics2D.OverlapBox
            (attackPoint.position, boxSize, 0f, targetLayer);

        if (hit != null)
        {
            HealthPointManager targetHp = hit.GetComponent<HealthPointManager>();

            if(targetHp != null)
            {
                int rawDamage = stars.GetAttackDamage(); //공격자 데미지
                targetHp.TakeDamage(rawDamage, gameObject); 
                //기존에 방어력 적용하고 넘기던 방식에서 방어력을 맞는사람 쪽에서 계산하게 바꿈
                //이유 : 여기서 방어력 1번 적용되고 -> 맞는쪽에서 방어력이 1번 더 적용되서
                         
            }     
      
        }
    }
    private void AttackPositionDirection()
    {
        if(attackPoint == null || movement == null) //방어코드
        {
            return;
        }

        Vector3 localPos = attackPoint.localPosition;

        if (movement.IsFacingRight)
        {
            localPos.x = Mathf.Abs(localPos.x);
        }
        else
        {
            localPos.x = -Mathf.Abs(localPos.x);
        }

        attackPoint.localPosition = localPos;
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null || stars == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Vector2 boxSize = new Vector2(stars.AttackRange, attackHitBox.y);
        Gizmos.DrawWireCube(attackPoint.position, boxSize);
    }

}
