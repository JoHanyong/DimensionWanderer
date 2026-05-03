using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [Header("스킬 데이터")]
    [SerializeField] private SkillData skillData;
    //슬롯에 들어갈 스킬

    [Header("스킬 실행")]
    [SerializeField] private MonoBehaviour skillBehaviour;

    private IskillBehaviour skill;

    private float lastUseTime = -999f;
    // 바로 스킬 사용 할수 있게

    private void Awake()
    {
        skill = skillBehaviour as IskillBehaviour;
    }
    public bool CanUse(CharacterStats stats)
    {
        if (skillData == null)
        {
            return false;
            //스킬 데이터 없으면 사용불가능
        }
        if(skill == null)
        {
            return false;
            //스크립트 없으면 사용불가능
        }
        if(Time.time <  lastUseTime + skillData.cooldown)
        {
            return false;
            //쿨타임 중에는 사용불가능
        }
        if(stats != null && stats.CurrentMana < skillData.manaCost)
        {
            return false; //스탯이 있고 마나가 부족하면 사용 불가
        } 

        return true; //조건을 모두 만족하면 사용가능
    }
    public void Use(CharacterStats stats)
    {
        if (!CanUse(stats))
        {
            return;
        }
        if (stats != null)
        {
            stats.UseMana(skillData.manaCost);
            //마나 사용
        }
        skill.UseSkill(); //스킬 실행

        lastUseTime = Time.time; //마지막 시전시간 기록
    }
    public float GetCooldownRemaining()
    {
        if (skillData == null)
        {
            return 0f;
            //스킬 없으면 쿨타임 0으로 설정
        }
        float remain = (lastUseTime + skillData.cooldown) - Time.time; //남은 쿨타임 계산법

        return Mathf.Max(0f , remain);
        //0보다 작으면 0으로 고정
    }
    public float GetCooldownNormalized()
    {
        if(skillData == null || skillData.cooldown <= 0f)
        {
            return 0f;
        }

        return GetCooldownRemaining() / skillData.cooldown;
    }
    private SkillData GetSkillData()
    {
        return skillData;
    }
}
