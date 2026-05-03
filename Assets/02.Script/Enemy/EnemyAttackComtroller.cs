
using UnityEngine;

public class EnemyAttackComtroller : MonoBehaviour
{
    [SerializeField] private CharacterStats statSystem; //공격 쿨타임, 공격력 정보 가져오기
    [SerializeField] private MonoBehaviour attackBehaviour; //AI한테 물어봤습니다.  <== 근접 공격 or 원거리 공격 연결용

    [Header("애니메이션")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTriggerName = "AttackSlash";

    private IEnemyAttack attack;

    private float lastAttackTime = -999f;

    private void Awake()
    {
        if(statSystem == null)
        {
            statSystem = GetComponent<CharacterStats>();
        }

        attack = attackBehaviour as IEnemyAttack;
    }
    public void ExecuteAttack()//공격 실행
    {

        if(statSystem == null)
        {
            return;
        }
        if (attack == null)
        {
            Debug.Log("attacjBehaviour가 IEnemyAttack으로 연결되지 않았다.");
        }
        if(Time.time < lastAttackTime + statSystem.AttackCooldown) //공격 쿨타임 체크
        {
            return;
        }
        lastAttackTime = Time.time;

        if(animator != null)
        {
            animator.SetTrigger(attackTriggerName);
        }


        attack.ExecuteAttack();

        SoundManager.Instance.PlayEnemyAttackSFX();
    }
}
