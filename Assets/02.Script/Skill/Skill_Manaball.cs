using UnityEngine;

public class Skill_Manaball : MonoBehaviour, IskillBehaviour
{
    [Header("Ref")]
    [SerializeField] private CharacterStats stats;
    //공격사거리, 공격력
    [SerializeField] private PlayerMove2D playerMove;
    [SerializeField] private Transform firePoint;

    [SerializeField] private ProjectileBullet projectilePrefab;

    [Header("타겟")]
    [SerializeField] private LayerMask targetLayer;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (stats == null)
        {
            stats = GetComponent<CharacterStats>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void UseSkill()
    {
        if (projectilePrefab == null || firePoint == null || stats == null)
        {
            return; //필요한 참조가 없으면 스킬 중단하기
        }

       

        Vector3 spawnPos = firePoint.position;
        spawnPos.y += 0.4f;
        //생성 위치 조정 
        SoundManager.Instance.PlayManaBallSFX();


        ProjectileBullet bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity); //탄환 생성

        Vector2 direction = new Vector2(playerMove.FacingDirection, 0f);

        bullet.Initializee(ownerStats: stats, moveDirection: direction, targetLayer: targetLayer, maxDistance: stats.AttackRange);
        //탄환 초기화
    }
}
