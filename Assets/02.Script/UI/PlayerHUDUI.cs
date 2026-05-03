using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Image expFillImage;
    [SerializeField] private Image manaFilllmage;

    [Header("Ref")]
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private CharacterStats playerStats;
    [SerializeField] private HealthPointManager health;

    private void Awake()
    {
        if(levelSystem == null)
        {
            levelSystem = GetComponent<PlayerLevelSystem>();
        }
        if(playerStats == null)
        {
            playerStats = GetComponent<CharacterStats>();
        }
        if(health == null)
        {
            health = GetComponent<HealthPointManager>();
        }

    }
    private void Start()
    {
        if (health == null)
        {
            health = FindFirstObjectByType<HealthPointManager>();
        }

        if (health != null)
        {
            health.OnDamaged += UpdateHealthUI;

            UpdateHealthUI();
        }
    }
    private void Update()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        UpdateLevelText();
        UpdateHpBar();
        UpdateExpBar();
    }
    private void UpdateLevelText()
    {
        if (levelText == null || levelSystem == null) //방어코드
        {
            return; 
        }

        levelText.text = "Lv. " + levelSystem.CurrentLevel;
    }
    private void UpdateHpBar()
    {
        if (hpFillImage == null || health == null)
        {
            return;
        }

        float currentHp = health.CurrentHealth;
        float maxHp = playerStats.MaxHealth;

        if(maxHp <= 0)
        {
            hpFillImage.fillAmount = 0;
            return;
        }
        hpFillImage.fillAmount = currentHp / maxHp;
    }
    private void UpdateExpBar()
    {
        if(expFillImage == null || levelSystem == null)
        {
            return;
        }

        float currentExp = levelSystem.CurrentExp;
        float requiredExp = levelSystem.RequiredExp;

        if(requiredExp <= 0)
        {
            expFillImage.fillAmount = 0;
            return;
        }
        expFillImage.fillAmount= currentExp / requiredExp;
    }
    private void UpdateMnan()
    {
       if(manaFilllmage == null || playerStats == null)
        {
            return;
        }
       float currentMana = playerStats.CurrentMana;
       float maxMana = playerStats.MaxMana;

        if (maxMana <= 0)
        {
            manaFilllmage.fillAmount = 0;
            return;
        }
        manaFilllmage.fillAmount = currentMana / maxMana;
    }
    private void UpdateHealthUI()
    {
        hpFillImage.fillAmount = (float)health.CurrentHealth / health.GetComponent<CharacterStats>().MaxHealth;
    }
    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDamaged -= UpdateHealthUI;
        }
    }
}
