using Unity.VisualScripting;
using UnityEngine;
using static EnemyState;

public class EnemyFSMController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private EnemyDetectionController detection; //타켓 거리 감지용

    [SerializeField] private EnemyMover mover; //이동 처리용

    [SerializeField] private HealthPointManager health; //사망 여부

    [SerializeField] private EnemyAttackComtroller attackComtroller; //공격처리

    [Header("상태")]
    [SerializeField] private enemyState currentState = enemyState.Idle; //현재 몬스터 상태용

    private bool isChasing;

    private void Awake()
    {
        if(detection == null)
        {
            detection = GetComponent<EnemyDetectionController>();
        }
        if(mover == null)
        {
            mover = GetComponent<EnemyMover>();
        }
        if(health == null)
        {
            health = GetComponent<HealthPointManager>();
        }
        if (attackComtroller == null)
        {
            attackComtroller = GetComponent<EnemyAttackComtroller>();
        }
    }
    private void Update()
    {
        if(health != null && health.IsDead)
        {
            return;
        }
        if(detection == null || detection.Target == null)
        {
            HandleIdleState();
            return;
        }
        //플레이어와 거리 계산
        float distance = Vector2.Distance(transform.position, detection.Target.position);

        switch (currentState)
        {
            case enemyState.Idle:
                HandleIdleState(); //가만히 있기
                ChekTransition(distance); //상태 변경 체킄
                break;
            case enemyState.Chase:
                HandleChaseState(); //플레이어 추적
                ChekTransition(distance);
                break;
            case enemyState.Attack:
                HandeAttackState(); //공격 실행
                ChekTransition(distance);
                break;

        }

        
    }
    private void HandleIdleState()
    {
        //상태를 Idle로 두기 
        currentState = enemyState.Idle;

        if (mover != null)
        {
            mover.StopMove();
        }

    }
    private void HandleChaseState()
    {
        //상태를 Chase로 두기 
        currentState = enemyState.Chase;

        //플레이어가 있으면 그쪽으로 이동
        if (mover != null && detection != null && detection.Target != null)
        {
            mover.MoveToTarget(detection.Target.position);
        }
    }
    private void HandeAttackState()
    {
        //상태 Attack으로 설정
        currentState = enemyState.Attack;

        //공격 때 이동 멈춤
        if(mover != null)
        {
            mover.StopMove();
        }
        //공격 실행(핵심)
        if(attackComtroller != null)
        {
            attackComtroller.ExecuteAttack();
        }
    }
    private void ChekTransition(float distance)
    {
        //상태 전환 로직

        if(detection != null && distance <= detection.AttackDistance)//공격 범위 안이면 공격상태
        {
            currentState = enemyState.Attack;
            isChasing = true;
            return;
        }
        if(detection != null && distance <= detection.ChaseDistance)//추적 범위 안이면 추적
        {
            currentState = enemyState.Chase;
            isChasing = true;
            return;
        }
        if(detection != null && isChasing && distance <= detection.LoseDistance)//추격 중이면 조금 멀어져도 계속 추적
        {
            currentState = enemyState.Chase;
            return;
        }

        currentState = enemyState.Idle;
        isChasing = false; //너무 멀리가버리면 다시 대기
    }
}
