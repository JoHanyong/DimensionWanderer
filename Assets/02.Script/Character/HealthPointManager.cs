using System;
using UnityEngine;

public class HealthPointManager : MonoBehaviour, IDamageable
{
    [Header("Ref")]
    [SerializeField] private CharacterStats statSystem; //최대체력 방어력 가져오기

    [Header("Health")]
    [SerializeField] private int currentHealth; //현재 체력
    [SerializeField] private bool isDead; //현재 사망 여부

    [Header("시작 시 체력회복")]
    [SerializeField] private bool setFullHealth = true;

    [Header("Parry")]
    [SerializeField] private bool isParrying = false;
    //현재 패링 중인지 확인
    [SerializeField] private float parryDuraton = 1f;
    //패링 유지시간

    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 2f;
    private float parryEndTime = 0f;
    //패링 종료시간 


    private Animator animator;

    public event Action OnDamaged;
    public event Action OnDead;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        if (statSystem == null)
        {
            statSystem = GetComponent<CharacterStats>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    private void Start()
    {
        if (setFullHealth == true && statSystem != null)
        {
            currentHealth = statSystem.MaxHealth;
            isDead = false;

            Debug.Log("체력 초기화 완료 : " + currentHealth);

            OnDamaged?.Invoke();
        }
    }
    private void Update()
    {
        if(isParrying && Time.time > parryEndTime)
        {
            isParrying = false;
            //패링 시간 끝나면 자동 종료
        }
    }
    
    public void StartParry()
    {
        isParrying = true;
        parryEndTime = Time.time + parryDuraton;

        Debug.Log("패링!!!");
    }
    private void OnParrySuccess(GameObject attacker)
    {
        isParrying = false;

        Debug.Log($"{gameObject.name} 패링 성공!!카운터!!");

        if(animator  != null)
        {
            animator.SetTrigger("Parry");
        }
        if (attacker != null)
        {
            IDamageable damageable = attacker.GetComponent<IDamageable>();

            if (damageable != null && statSystem != null)
            {
                //카운터
                int counterDamage = Mathf.RoundToInt(statSystem.GetAttackDamage() * 2f);
                damageable.TakeDamage(counterDamage, gameObject);
                Debug.Log($"상대에게 카운터!! {counterDamage} 데미지");
            }
        }
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (isDead) // 이미 죽은 상태면 더 이상 데미지 안 받음
        {
            return;
        }
        if(statSystem == null) //스텟 정보가 없으면 중단
        {
            return;
        }
        if (isParrying)
        {
            OnParrySuccess(attacker);
            return;
        }

        
        int finalDamage = statSystem.GetFinalDamage(damage);

        Debug.Log($"{gameObject.name} 데미지 받음 / 들어온 데미지 : {damage} / 최종데미지 {finalDamage}");

        currentHealth -= finalDamage;

        SoundManager.Instance.PlayHitSFX();

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if(currentHealth <= 0)
        {
            Die();
            return;
        }
        if(animator != null)
        {
            animator.SetTrigger("Hit");
        }
        OnDamaged?.Invoke();

    }
    public void Heal(int amount)
    {
        if (isDead)
        {
            return;
        }
        if (statSystem == null)
        {
            return;
        }

        currentHealth += amount;
        
        if(currentHealth > statSystem.MaxHealth) //최대 체력 이상 회복막기
        {
            currentHealth = statSystem.MaxHealth;
        }

        OnDamaged?.Invoke();
    }
    private void Die() //죽음 애니메이션
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        SoundManager.Instance.PlayEnemyDeathSFX();
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        OnDead?.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
    public int MaxHealth
    {
        get
        {
            if (statSystem == null)
            {
                return 1;
            }

            return statSystem.MaxHealth;
        }
    }
    public void ResetHealth()
    {
        if (statSystem == null)
        {
            statSystem = GetComponent<CharacterStats>();
        }

        currentHealth = statSystem.MaxHealth;
        isDead = false;
        isParrying = false;
        parryEndTime = 0f;

        OnDamaged?.Invoke();

        Debug.Log("체력 초기화 완료 : " + currentHealth);
    }
}
