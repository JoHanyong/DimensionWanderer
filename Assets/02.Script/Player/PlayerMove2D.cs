using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]//Rigidbody2D 없으면 작동안됨
public class PlayerMove2D : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerInputReader inputReader;
    private Animator animator;


    [Header("Move")]
    [SerializeField] private CharacterStats statSystem; //이동 속도
    [SerializeField] private SpriteRenderer spriteRenderer; //스프라이트 반전


    [Header("Jump")]
    [SerializeField] private float jumpForce = 14f;// 높이

    [Header("Dash")]
    [SerializeField] private float dashPower = 16f; //대쉬 파워->거리
    [SerializeField] private float dashDuration = 0.15f; //대쉬 유지시간
    [SerializeField] private float dashCooldown = 0.5f; //대쉬 쿨타임

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float CheckFloor = 0.15f; //바닥 체크
    [SerializeField] private LayerMask groundLayer; //바닥 레이어

    

    private Rigidbody2D rb;
    //점프 
    private bool isGrounded; //바닥에 있는지
    private bool isFacingRight = true; 
    private bool isDashing; //점프 중에 대쉬 중인지
    //대쉬
    private float dashTimer; //대쉬 남은 시간
    private float dashCooldownTimer; //쿨타임
    private float SacetheOriginalGravityValue; //원래 중력값 저장

    public bool IsFacingRight => isFacingRight;
    public int FacingDirection => isFacingRight ? 1 : -1; //방향을 숫자로 변환 X축 기준 오른쪽 +1 왼쪽 -1

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SacetheOriginalGravityValue = rb.gravityScale;

        if(inputReader == null)
        {
            inputReader = GetComponent<PlayerInputReader>();
        }
        animator = GetComponent<Animator>();

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    private void Update()
    {
        CheckGround(); 
        UpdateFacing(); 
        UpdateDashTimer(); 
        HandleJump(); 
        HandleDash(); 
       
    }
    private void FixedUpdate()
    {
        // 대쉬 중이면 이동 막기
        if (isDashing)
        {
            return;
        }
        

        HandleMove();
    }
    private void HandleMove()
    {
        
        float moveX = inputReader.MoveX;
        
        
        rb.linearVelocity = new Vector2(moveX * statSystem.MoveSpeed, rb.linearVelocity.y); //y는 유지 x속도만 입력

        bool isRunning = Mathf.Abs(moveX) > 0.01f;
        animator.SetBool("isRun", isRunning);

        
    }
    private void HandleJump() //입력 점프
    {
        
        if (!inputReader.JumpPressedThisFrame)
        {
            return;
        }
        if (!isGrounded)
        {
            return ;
        }
        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, jumpForce);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayJumpSFX();
        }
    }
    private void HandleDash() //입력 대쉬
    {
        
        if (isDashing)
        {
            return;
        }
        if (!inputReader.DashPressedThisFrame)
        {
            return;
        }
        if (dashCooldownTimer > 0f)
        {
            return;
        }

        StartDash();
    }
    private void StartDash() 
    {
        isDashing = true; //대쉬 상태
        dashTimer = dashDuration; // 대쉬 시간 설정
        dashCooldownTimer = dashCooldown; //쿨타임

        rb.gravityScale = 0f; //중력 제거 

        rb.linearVelocity = new Vector2(FacingDirection * dashPower, 0f);

        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayDashSFX();
        }
    }
    private void UpdateDashTimer()
    {
        if(dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (!isDashing)
        {
            return;
        }

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }
    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = SacetheOriginalGravityValue; // 중력 복구

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
    private void UpdateFacing()
    {
        float moveX = inputReader.MoveX;

        if(inputReader.MoveX > 0.01f)
        {
            isFacingRight = true;

            if (spriteRenderer  != null)
            {
                spriteRenderer.flipX = true; //오른쪽
            }
        }
        else if (inputReader.MoveX < -0.01f)
        {
            isFacingRight = false;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false; //왼쪽
            }
        }
    }
    private void CheckGround()
    {
        if(groundCheckPoint == null)
        {
            isGrounded = false;
            return;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, CheckFloor, groundLayer);
        
    }
    
}


