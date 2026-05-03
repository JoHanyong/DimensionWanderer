
using UnityEngine;

public class AugmentSelectionUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot; //전체패널
    [SerializeField] private AugmentCardUI[] cards; //카드 3개 배열 

    [Header("Ref")]
    [SerializeField] private AugmentSystem augmentSystem; // 실제 증강 로직 

    private void Awake()
    {
        if(panelRoot != null)
        {
            panelRoot.SetActive(false); //시작 할때 UI 꺼두기
        }
    }
    private void Start()
    {
        
    }
    private void ForceCloseUI()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        Time.timeScale = 1f;
    }
    private void OnEnable()
    {
        FindPlayerAugmentSystem();

        if (augmentSystem != null)
        {
            augmentSystem.OnAugmentSelectionOpened -= OpenUI;
            augmentSystem.OnAugmentSelectionOpened += OpenUI;

            augmentSystem.OnAugmentSelectionClosed -= CloseUI;
            augmentSystem.OnAugmentSelectionClosed += CloseUI;
        }
    }
    private void OnDisable()
    {    
        //이벤트 해제(중복 방지)
        if(augmentSystem != null)
        {
            augmentSystem.OnAugmentSelectionOpened -= OpenUI;
            augmentSystem.OnAugmentSelectionClosed -= CloseUI;
        }
    }

    public void OpenUI()
    {
        FindPlayerAugmentSystem();

        Debug.Log("증강 UI OpenUI 실행됨");

        if (panelRoot == null)
        {
            Debug.LogError("PanelRoot 비어있음");
            return;
        }
        panelRoot.SetActive(true);

        SoundManager.Instance.PlayAugmentOpenSFX();

        Time.timeScale = 0f;
        RefreshCards();
    }
    public void CloseUI() //UI 닫기
    {
        if(panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        Time.timeScale = 1f; //게임 다시 시작
    }
    public void RefreshCards()
    {
        if(augmentSystem == null || cards == null)
        {
            return;
        }

        for (int i = 0; i < cards.Length; i++) //현재 표시할 증강이 있는 경우
        {
            if (i < augmentSystem.CurrentOfferedAugments.Count)
            {
                AugmentData data = (AugmentData)augmentSystem.CurrentOfferedAugments[i];

                bool canReroll = !augmentSystem.HasUsedRerollOnSlot(i);

                cards[i].gameObject.SetActive(true);
                cards[i].Setup(data, i, this, canReroll);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
        
    }
    public void SelectCard(int index) //카드 선택하기
    {
        SoundManager.Instance.PlayButtonSFX();

        if (augmentSystem == null)
        {
            return; 
        }

        augmentSystem.SelectAugment(index); //실제 증강 적용
    }
    public void RerollCard(int index)
    {

        Debug.Log("현재 증강 카드 개수 : " + augmentSystem.CurrentOfferedAugments.Count);

        SoundManager.Instance.PlayButtonSFX();
        //카드 리롤
        if (augmentSystem == null)
        {
            return;
        }
        augmentSystem.RerollSlot(index); //카드 다시 뽑고 UI 갱신
        RefreshCards();
    }
    private void FindPlayerAugmentSystem()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾지 못했습니다.");
            augmentSystem = null;
            return;
        }

        augmentSystem = player.GetComponent<AugmentSystem>();

        if (augmentSystem == null)
        {
            Debug.LogWarning("Player에 AugmentSystem이 없습니다.");
        }
        else
        {
            Debug.Log("AugmentSystem 재연결 완료 : " + player.name);
        }
    }
}
