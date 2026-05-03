using System;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    [Header("레벨 정보")]
    [SerializeField] private int currentLevel = 1; //현재 레벨
    [SerializeField] private int currentExp = 0; //현재 경험치
    [SerializeField] private int requiredExp = 10; //레벨업에 필요한 경험치

    [Header("레벨 증가 규칙")]
    [SerializeField] private int expIncreasePerLevel = 5; //레벨업마다 필요 경험치 증가량

    [Header("Ref")]
    [SerializeField] private CharacterStats playerStats; //렙업 시 스탯증가
    [SerializeField] private HealthPointManager health; //레벨업 시 체력 회복?


    public event Action<int> OnLevelChanged;

    private int baseRequiredExp; //레벨 저장 

    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int RequiredExp => requiredExp;
    private void Awake()
    {
        baseRequiredExp = requiredExp;
        if (playerStats == null)
        {
            playerStats = GetComponent<CharacterStats>();
        }
        if(health == null)
        {
            health = GetComponent<HealthPointManager>();
        }
    }
    public void ResetLevel()
    {
        currentLevel = 1;
        currentExp = 0;
        requiredExp = baseRequiredExp;

        OnLevelChanged?.Invoke(currentLevel);

        Debug.Log("레벨/경험치 초기화 완료");
    }
    public void AddExp(int amount)
    {
        if(amount <= 0)
        {
            return;
        }

        currentExp += amount;
        Debug.Log($"경험치 획득: {amount} / 현재 EXP : {currentExp}");

        while(currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUP();
        }
    }
    private void LevelUP()
    {
        currentLevel++;  //레벨 증가
        requiredExp += expIncreasePerLevel; //다음 레벨 요구치 증가

        Debug.Log($"레벨업! 현재 레벨: {currentLevel}");

        SoundManager.Instance.PlayLevelUpSFX();
        

        ApplyLevelUpReward();

        OnLevelChanged?.Invoke(currentLevel); //외부 시스템에 알려주기

        
        if (health != null)
        {
            health.Heal(50);
        }

        OnLevelChanged?.Invoke(currentLevel);
    }
    private void ApplyLevelUpReward()
    {
        if(playerStats != null)
        {
            int attackBonus = UnityEngine.Random.Range(1, 6); //랜덤 공격력 보너스 1~5 부여
            float defenseBonus = UnityEngine.Random.Range(1f, 3f); //1.0~3.0 보너스부여
            int HealthBonus = UnityEngine.Random.Range(10, 21); //체력
            float criticalBonus = UnityEngine.Random.Range(0.5f, 2f); //크리티컬
            int criticalDamageBonus = UnityEngine.Random.Range(5, 16); //크리티컬데미지

            playerStats.AddBonusAttack(attackBonus);
            playerStats.AddBonusDefense(defenseBonus);
            playerStats.AddBonusHealth(HealthBonus);
            playerStats.AddBonusCritical(criticalBonus);
            playerStats.AddBonusCriticalDamage(criticalDamageBonus);

            Debug.Log($"레벨 업!!보너스 -> 공격:{attackBonus}, 방어:{defenseBonus}, 체력:{HealthBonus}, 크리티컬:{criticalBonus}, 크리티컬 데미지:{criticalDamageBonus}");
        }
    }
}
