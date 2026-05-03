using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarUI : MonoBehaviour
{
    [SerializeField] private Image hpFillImage;
    [SerializeField] private HealthPointManager health;
    [SerializeField] private CharacterStats Stats;
    private void Awake()
    {
        if(health == null)
        {
            health = GetComponent<HealthPointManager>();
        }
        if(Stats == null)
        {
            Stats = GetComponent<CharacterStats>(); 
        }
    }
    private void Update()
    {
        RefreshHpBar();
        CheckDead();
    }
    private void RefreshHpBar()
    {
        if (hpFillImage == null || health == null || Stats == null)
        {
            return;
        }
        float currentHp = health.CurrentHealth;
        float maxHp = Stats.MaxHealth;

        if (maxHp <= 0)
        {
            hpFillImage.fillAmount = 0;
            return;
        }

        hpFillImage.fillAmount = currentHp / maxHp;
    }
    private void CheckDead() 
    {
        if(health != null && health.IsDead)
        {
            gameObject.SetActive(false);
        }
    }
}
