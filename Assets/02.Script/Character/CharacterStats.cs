using System.Data.Common;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] private int maxHealth = 100; //체
    [SerializeField] private int attackPower = 10; //공
    [SerializeField] private float moveSpeed = 3f; //스
    [SerializeField] private float defense = 0f; //방

    [Header("공격")]
    [SerializeField] private float attackCooldown = 1f; //공 쿨
    [SerializeField] private float attackRange = 1.5f; // 공 사거리

    [Header("크리티컬")]
    [SerializeField] private float critical = 5f; //크리티컬 5%
    [SerializeField] private int criticalDamage = 50; //크리티컬 데미지(공100 + 50 더해지는 방식)

    [Header("마나")]
    [SerializeField] private float maxMana = 100f; //최대 마나
    [SerializeField] private float currentMana;
    [SerializeField] private float manaRegenPerSecond = 5f;

    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;
    public float MoveSpeed => moveSpeed;
    public float Defense => defense;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;
    public float Critical => critical;
    public float CriticalDamage => criticalDamage;

    public float MaxMana => maxMana;
    public float CurrentMana => currentMana;

    private int baseMaxHealth; //저장용
    private int baseAttackPower;
    private float baseMoveSpeed;
    private float baseDefense;
    private float baseAttackCooldown;
    private float baseAttackRange;
    private float baseCritical;
    private int baseCriticalDamage;
    private float baseMaxMana;

    private void Awake()
    {
        baseMaxHealth = maxHealth;
        baseAttackPower = attackPower;
        baseMoveSpeed = moveSpeed;
        baseDefense = defense;
        baseAttackCooldown = attackCooldown;
        baseAttackRange = attackRange;
        baseCritical = critical;
        baseCriticalDamage = criticalDamage;
        baseMaxMana = maxMana;

        currentMana = maxMana;
        //시작 할 때 현재 마나 최대마나하고 똑같이 하기
    }
    private void Update()
    {
        RegenerateMana();
    }
    private void RegenerateMana()
    {
        currentMana += manaRegenPerSecond * Time.deltaTime;
        currentMana = Mathf.Min(currentMana, maxMana);
    }
    public void ResetStats()
    {
        maxHealth = baseMaxHealth;
        attackPower = baseAttackPower;
        moveSpeed = baseMoveSpeed;
        defense = baseDefense;
        attackCooldown = baseAttackCooldown;
        attackRange = baseAttackRange;
        critical = baseCritical;
        criticalDamage = baseCriticalDamage;
        maxMana = baseMaxMana;
        currentMana = maxMana;

        Debug.Log("스탯 초기화 완료");
    }
    public int GetFinalDamage(int incomingDamge)
    {
        int defenseValue = Mathf.RoundToInt(defense);

        int finalDamage = incomingDamge - defenseValue;

        if(finalDamage < 1)
        {
            finalDamage = 1;
        }

        return finalDamage;

    }
    public bool IsCriticalHit()
    {
        return Random.Range(0f, 100f) < critical; //확률 보다 작으면 크리티컬
    }
    public int GetAttackDamage()
    {
        int finalDamage = attackPower;

        if (IsCriticalHit()) 
        {
            finalDamage += criticalDamage;
        }

        return finalDamage;
    }
    public bool HasEnoughMana(int amount)
    {
        return currentMana >= amount;
        //현재 마나가 필요 마나 이상인지 확인하기
    }
    public void UseMana(int amount)
    {
        currentMana -= amount;
        //마나 사용
        if (currentMana < 0)
        {
            currentMana = 0;
        }
        Debug.Log($"마나 사용, 현재 마나: {currentMana} / {maxMana}");
    }
    public void RecoverMana(int amount)
    {
        currentMana += amount;
        //마나 회복

        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        Debug.Log($"마나 회복, 현재 마나: {currentMana} / {maxMana}");
    }
    public void AddBonusAttack(int amount) 
    {
        attackPower += amount;
        Debug.Log($"공격력 증가: {attackPower}");
    }
    public void AddBonusDefense(float amount)
    {
        defense += amount;
        Debug.Log($"방어력 증가: {defense}");
    }
    public void AddBonusHealth(int amount)
    {
        maxHealth += amount;
        Debug.Log($"체력 증가: {maxHealth}");
    }
    public void AddBonusCritical(float amount)
    {
        critical += amount;
        Debug.Log($"크리티컬 증가: {critical}");
    }
    public void AddBonusCriticalDamage(int amount)
    {
        criticalDamage += amount;
        Debug.Log($"크리티컬 데미지 증가: {criticalDamage}");
    }
    public void AddBonusSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log($"이동속도 증가 {moveSpeed}");
    }
        
}
