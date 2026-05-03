using UnityEngine;

public class AugmentData : MonoBehaviour
{
    [Header("기본정보")]
    [SerializeField]
    public string augmentId; //증강 고유 ID
    public string augmentName; //증강 이름
    [TextArea(2, 5)]
    public string description; //설명
    public AugmentTier tier; // 등급

    [Header("능력치 보너스")]
    public int attackBonus;
    public float defenssBonus;
    public int maxHealthBonus;
    public float moveSpeedBonus;
    public float criticalBonus;
    public int criticalDamageBonus;

    // 증강 효과적용
    public void ApplyTo(CharacterStats stats, HealthPointManager health)
    {
        if(stats != null)
        {
            if (attackBonus != 0)
            {
                stats.AddBonusAttack(attackBonus);
            }
            if (defenssBonus != 0)
            {
                stats.AddBonusDefense(defenssBonus);
            }
            if (maxHealthBonus != 0)
            {
                stats.AddBonusHealth(maxHealthBonus);
            }
            if (criticalBonus != 0)
            {
                stats.AddBonusCritical(criticalBonus);
            }
            if (criticalDamageBonus != 0)
            {
                stats.AddBonusCriticalDamage(criticalDamageBonus);
            }
            if (moveSpeedBonus != 0)
            {
                stats.AddBonusSpeed(moveSpeedBonus);
            }
        }
    }
}
