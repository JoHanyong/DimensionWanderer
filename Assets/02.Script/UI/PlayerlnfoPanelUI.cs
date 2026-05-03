using System.Text;
using TMPro;
using UnityEngine;

public class PlayerlnfoPanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot; //전체패널
    [SerializeField] private TMP_Text levelText; //레벨
    [SerializeField] private TMP_Text expText; //경험치
    [SerializeField] private TMP_Text hpText; //체력
    [SerializeField] private TMP_Text attackText; //공격력
    [SerializeField] private TMP_Text defenseText; //방어력
    [SerializeField] private TMP_Text speedText; //이동속도
    [SerializeField] private TMP_Text criticalText; //크리티컬
    [SerializeField] private TMP_Text criticalDamageText; //크리티컬 확률
    [SerializeField] private TMP_Text ownedAugmentText; //보유 증강

    [Header("Ref")]
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private AugmentSystem augmentSystem;
    [SerializeField] private HealthPointManager health;
    
    private bool isOpen = false; //창 열림 상태

    private void Awake() //시작 할때 꺼두기
    {
        if(panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }
    private void Start()
    {
        FindPlayerReferences();
    }
    private void OnEnable()
    {
        FindPlayerReferences();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel();
        }
    }
    private void FindPlayerReferences()
    {
        GameObject player = GameObject.FindWithTag("Player");

        levelSystem = player.GetComponent<PlayerLevelSystem>();
        stats = player.GetComponent<CharacterStats>();
        augmentSystem = player.GetComponent<AugmentSystem>();
        health = player.GetComponent<HealthPointManager>();
    }
    public void TogglePanel()
    {
        isOpen = !isOpen; 

        if (panelRoot != null)
        {
            panelRoot.SetActive(isOpen);
        }
        if (isOpen)
        {
            RefreshInfo();
        }
    }
    public void RefreshInfo() //정보 업데이트 
    {
        levelText.text = "레벨 : " + levelSystem.CurrentLevel;

        expText.text = "경험치 : " + levelSystem.CurrentExp + " / " + levelSystem.RequiredExp;

        hpText.text = "체력 : " + health.CurrentHealth + " / " + stats.MaxHealth;

        //스탯
        attackText.text = "공격력 : " + stats.AttackPower;
        defenseText.text = "방어력 : " + stats.Defense;
        speedText.text = "이동속도 : " + stats.MoveSpeed;
        criticalText.text = "치명타 : " + stats.Critical + "%";
        criticalDamageText.text = "치명타 데미지 : +" + stats.CriticalDamage;

        UpdateOwnedAugmentList(); //증강 목록 업데이트
    }
    private void UpdateOwnedAugmentList() //보유 증강 목록 
    {
        if (ownedAugmentText == null || augmentSystem == null)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("보유 증강");

        if(augmentSystem.OwnedAugments.Count == 0) //보유증강 없음
        {
            sb.AppendLine("-보유 증강이 없습니다.");
        }
        else
        {
            for(int i = 0; i < augmentSystem.OwnedAugments.Count; i++) //리스트 출력하기
            {
                AugmentData data = (AugmentData)augmentSystem.OwnedAugments[i];

                if(data == null)
                {
                    continue;
                }

                sb.AppendLine("- " + data.augmentName);
            }
        }
        ownedAugmentText.text = sb.ToString();
    }
    

}
