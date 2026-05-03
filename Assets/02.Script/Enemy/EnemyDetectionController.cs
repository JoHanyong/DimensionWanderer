using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class EnemyDetectionController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    //추적할 대상

    [Header("Distance")]
    [SerializeField] private float attackDistance = 1.5f;
    // 공격 범위
    [SerializeField] private float chaseDistance = 5f;
    //어그로 범위
    [SerializeField] private float loseDistance = 6f;
    //어그로 범위 벗어나는 범위 

    public Transform Target => target;
    public float AttackDistance => attackDistance;
    public float ChaseDistance => chaseDistance;
    public float LoseDistance => loseDistance;
    public bool HasTarget => target != null; // 타켓존재여부

    private void Start()
    {
        FindTarget();
    }
    private void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
            Debug.Log("Detection Target 재설정 완료 : " + target.name);
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾지 못함");
        }
    }
    public void SetTarget(Transform newTarget) // 외부에서 타켓 설정하는 함수
    {
        target = newTarget; 
    }
    public float GetDistanceToTarget()
    {
        if(target == null)
        {
            return Mathf.Infinity;   
        }
        return Vector2.Distance(transform.position, target.position);
        // 현재 오브젝트 - 타켓 사이 거리 계산
    }
    public bool IsInAttackRange()
    {
        return GetDistanceToTarget() <= attackDistance;
        //공격 가능한 거리 안
    }
    public bool IsInChaseRange()
    {
        return GetDistanceToTarget() <= chaseDistance;
        //추적 시작 거리 안
    }
    public bool IsOutOfLoseRange()
    {
        return GetDistanceToTarget() > loseDistance;
        //추적 포기 거리 밖
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance); //빨간 원 (공격 범위)

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance); //노랑 원 (추적 시작 범위)

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseDistance); //희색 원 (추적 해제 범위)
    }

}
