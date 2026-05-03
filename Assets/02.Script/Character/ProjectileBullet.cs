using UnityEngine;
[RequireComponent (typeof(Collider2D))]
public class ProjectileBullet : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 8f; //탄환 이동 속도

    private CharacterStats ownerStats; //공격자 정보
    private Vector2 moveDirection; //이동방향
    private LayerMask targetLayer;
    private float maxDistance; //최대 이동거리

    private Vector3 startPosition;
    private bool isTnitialized;

    public void Initializee(CharacterStats ownerStats, Vector2 moveDirection, LayerMask targetLayer, float maxDistance)
    {
        this.ownerStats = ownerStats;
        this.moveDirection = moveDirection.normalized;
        this.targetLayer = targetLayer;
        this.maxDistance = maxDistance;
        
        //시작 위치 저장
        startPosition = transform.position;

        isTnitialized = true;

        //방향 따라서 스프라이트 반전
        RotateToDirection(); 
    }

    private void Update()
    {
        if(!isTnitialized)
        {
            return;
        }
        MoveProjectile();

        CheckDistance();


    }
    private void MoveProjectile()
    {
        //방향 * 속도 * 시간 -> 이동
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }
    private void CheckDistance()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        //현재 위치 시작위치 거리 계산
        if(distance >= maxDistance)
        {
            Destroy(gameObject); //최대거리 넘으면 삭제
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTnitialized)
        {
            return;
        }
        if(((1 << collision.gameObject.layer) & targetLayer) == 0)
        {
            return;
        }

       
        HealthPointManager targetHp = collision.GetComponent<HealthPointManager>();
        //맞는 대상 체력

        if(targetHp != null && ownerStats != null)
        {
            //공격자 공격력
            int damage = ownerStats.GetAttackDamage();

            

            //체력 감소
            targetHp.TakeDamage(damage, gameObject);
        }
        Destroy(gameObject); //맞으면 투사체 삭제
    }
    private void RotateToDirection()
    {

        Vector3 scale = transform.localScale;

        if (moveDirection.x > 0f)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else if (moveDirection.x < 0f)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;   
    }

}
