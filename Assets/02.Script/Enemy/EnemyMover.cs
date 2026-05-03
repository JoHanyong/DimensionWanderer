using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CharacterStats statSystem;
    //이동 속도 가져오기
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MeleeAttack meleeAttack;

    public bool CanMove { get; set; } = true; //현재 이동가능한지 

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (statSystem == null)
        {
            statSystem = GetComponent<CharacterStats>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();    
        }
        if (meleeAttack == null)
        {
            meleeAttack = GetComponent<MeleeAttack>();
        }
    }
    public void MoveToTarget(Vector2 targetPosition)
    {
        if(!CanMove || statSystem  == null)
        {
            StopMove();
            return;
        }
        Vector2 direction = (targetPosition - rb.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * statSystem.MoveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(direction.x) > 0.05f && spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x > 0f;
        }
        bool isRight = direction.x > 0f;

        // 스프라이트 방향 변경
        spriteRenderer.flipX = isRight;

        // 공격 방향도 같이 변경 (중요)
        if (meleeAttack != null)
        {
            meleeAttack.SetFacingDirection(isRight);
        }
    }
    public void StopMove()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        //x축은 멈추고 y축은 유지
    }
}
