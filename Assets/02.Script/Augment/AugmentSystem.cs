
using System;
using System.Collections.Generic;
using UnityEngine;

public class AugmentSystem : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private PlayerLevelSystem levelSystem;
    [SerializeField] private CharacterStats playerStats;
    [SerializeField] private HealthPointManager health;

    [Header("증강 등장 레벨")]
    [SerializeField] private int[] augmentOpenLevels = { 3, 5, 7, 11, 13, 15 };

    [Header("등급별 증강 풀")]
    [SerializeField] private List<AugmentTierPool> tierPools = new List<AugmentTierPool>();

    [Header("등급 확률")]
    [Range(0f, 100f)][SerializeField] private float bronzeChance = 34f;
    [Range(0f, 100f)][SerializeField] private float silverChance = 33f;
    [Range(0f, 100f)][SerializeField] private float goldChance = 33f;

    [Header("카드 설정")]
    [SerializeField] private int offerCount = 3;
    //key 등급, Aalue = 해당 등급의 증강 리스트
    private Dictionary<AugmentTier, List<AugmentData>> augmentPoolByTier =
        new Dictionary<AugmentTier, List<AugmentData>>();

    //현재 증강 화면에 떠 있는 카드 넣는곳
    private List<AugmentData> currentOfferedAugments = new List<AugmentData>();
    //플레이어가 이미 획득한 증강들 넣는곳
    private List<AugmentData> ownedAugments = new List<AugmentData>();
    //처리 중인 증강 이벤트 (이벤트 실행 레벨 저장)
    private Queue<int> pendingAugmentLevels = new Queue<int>(); 
    //리롤 직전 카드 상태 저장 
    private Stack<List<AugmentData>> rerollHistory = new Stack<List<AugmentData>>();
    private AugmentTier currentOfferTier; //현재 선택창에 적용된 등급

    private bool isChoosingAugment = false; //증강 선택 중인지
    private bool[] rerolledThisRound = new bool[3];
    //현재 레벨에서 이미 큐에 넣었는지 중복 방지용
    private HashSet<int> enqueuedLevels = new HashSet<int>();

    public IReadOnlyList<AugmentData> CurrentOfferedAugments => currentOfferedAugments;
    public IReadOnlyList<AugmentData> OwnedAugments => ownedAugments;
    public AugmentTier CurrentOfferTier => currentOfferTier;

    public event Action OnAugmentSelectionOpened;
    public event Action OnAugmentSelectionClosed;

    public bool IsChoosingAugment => isChoosingAugment;



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

        BuildDictionaryFromInspectorData(); //데이터 초기화

        if (rerolledThisRound == null || rerolledThisRound.Length != offerCount)
        {
            rerolledThisRound = new bool[offerCount];
        }
    }
    public void ResetAugmentSystem()
    {
        currentOfferedAugments.Clear();
        ownedAugments.Clear();
        pendingAugmentLevels.Clear();
        rerollHistory.Clear();
        enqueuedLevels.Clear();

        isChoosingAugment = false;

        for (int i = 0; i < rerolledThisRound.Length; i++)
        {
            rerolledThisRound[i] = false;
        }

        OnAugmentSelectionClosed?.Invoke();

        Debug.Log("증강 시스템 초기화 완료");
    }
    private void OnEnable()
    {
        if(levelSystem != null)
        {
            levelSystem.OnLevelChanged += HandLeLevelChanged;
        } 
    }
    private void OnDestroy()
    {
        if(levelSystem != null)
        {
            levelSystem.OnLevelChanged -= HandLeLevelChanged;
        }
    }
    private void BuildDictionaryFromInspectorData()
    {
        augmentPoolByTier.Clear(); //데이터 초기화 

        foreach (var pool in tierPools)
        {
            if (pool == null)
            {
                continue;
            }
            if (!augmentPoolByTier.ContainsKey(pool.tier)) //해당 티어 키 없으면 새로생성하기
            {
                augmentPoolByTier[pool.tier] = new List<AugmentData>();
            }
            if(pool.augments != null) //리스트가 있으면 추가
            {
                augmentPoolByTier[pool.tier].AddRange(pool.augments);
            }
        }
    }
    private void HandLeLevelChanged(int newLevel)
    {
        for (int i = 0; i < augmentOpenLevels.Length; i++) //배열 돌면서 현재 레벨이 증강 등장 레벨인지 확인하기 위해서
        {
            if(newLevel == augmentOpenLevels[i])
            {
                if (!enqueuedLevels.Contains(newLevel)) //중복 방지 
                {
                    pendingAugmentLevels.Enqueue(newLevel);
                    enqueuedLevels.Add(newLevel);

                    Debug.Log($"증강 이벤트 큐 추가: 레벨 {newLevel}");
                }
            }
        }
        TryprocessNextAugmentEvent();
    }
    private void TryprocessNextAugmentEvent() //증강 선택 중이 아니면 Queue에서 다음 이벤트 꺼내오기
    {
        if (isChoosingAugment)
        {
            return;
        }
        if (pendingAugmentLevels.Count <= 0)
        {
            return ;
        }
        int triggerLevel = pendingAugmentLevels.Dequeue();
        Debug.Log($"증강 이벤트 시작: 레벨 {triggerLevel}");

        OpenAugmentSelection();
    }
    private void OpenAugmentSelection() //증강 선택창 열기
    {
        isChoosingAugment = true;

        //데이터 초기화 (라운드)
        currentOfferedAugments.Clear();
        rerollHistory.Clear();

        for (int i = 0; i < rerolledThisRound.Length; i++)
        {
            rerolledThisRound[i] = false;
        }

        currentOfferTier = RollTierByChance(); //랜덤 등급 결정
        
        GenerateNewOffer(currentOfferTier); //랜덤으로 정해진 등급에서 카드3장 뽑기

        OnAugmentSelectionOpened?.Invoke();

        Debug.Log($"증강 선택창 오픈 / 등급 : {currentOfferTier}");
    }
    private AugmentTier RollTierByChance()
    {
        float total = bronzeChance + silverChance + goldChance; //확률 각각 34,33,33 총합 확률 100%
        float roll = UnityEngine.Random.Range(0f, total); //<-3등급으로 랜덤 돌리기

        if (roll < bronzeChance)
        {
            return AugmentTier.Bronze;
        }
        roll -= bronzeChance;

        if(roll < silverChance)
        {
            return AugmentTier.Silver;
        }
        roll -= silverChance;

        return AugmentTier.Gold;
    }
    private void GenerateNewOffer(AugmentTier tier) //현 등급에서 카드3개 생성
    {
        currentOfferedAugments.Clear();

        //후보군 만들기
        List<AugmentData> candidates = BuildCandidateList(tier, currentOfferedAugments);

        //최대 offerCount 개수 만큼 뽑기 카드 수 0이면 멈춤
        for(int i = 0; i < offerCount; i++)
        {
            if(candidates.Count <= 0)
            {
                break;
            }

            int randomIndex = UnityEngine.Random.Range(0, candidates.Count);
            AugmentData picked = candidates[randomIndex];

            currentOfferedAugments.Add(picked);
            candidates.RemoveAt(randomIndex); //같은 화면 중복 방지
        }
    }
    //해당 등급 풀, 이미 가진 증강 제외, 현재 선택화면에 있는 증강 제외한 카드한 나머지 카드
    private List<AugmentData> BuildCandidateList(AugmentTier tier, List<AugmentData> currentList)
    {
        List<AugmentData> candidates = new List<AugmentData>();

        if (!augmentPoolByTier.ContainsKey(tier))
        {
            return candidates;
        }
        List<AugmentData> sourcePool = augmentPoolByTier[tier];

        for (int i = 0; i < sourcePool.Count; i++)
        {
            AugmentData data = sourcePool[i];

            if(data == null)
            {
                continue;
            }
            if(ContainsAugmentById(ownedAugments, data.augmentId)) // 이미 보유햐나 증강이면 제외
            {
                continue;
            }
            if(ContainsAugmentById(currentList,data.augmentId)) // 현재화면에 있는 카드면 제외 <-증강 선택화면에서 떠 있는 카드3장 말해요!
            {
                continue;
            }
            candidates.Add(data);
            
        }
        return candidates;  
    }
    private bool ContainsAugmentById(List<AugmentData> list, string targetId) //리스트 안에 같은 ID 증강 있는지 확인하기
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i].augmentId == targetId)
            {
                return true;
            }
        }
        return false;
    }
    public void SelectAugment(int index)
    {
        if (!isChoosingAugment)
        {
            return;
        }

        if(index < 0 || index >= currentOfferedAugments.Count)
        {
            return;
        }
        AugmentData selected = currentOfferedAugments[index];
        if (selected == null)
        {
            return;
        }

        SoundManager.Instance.PlayAugmentSelectSFX();

        // 플레이어 보유 목록에 추가
        ownedAugments.Add(selected);

        //실제 효과 적용 
        selected.ApplyTo(playerStats, health);

        Debug.Log($"증강 선택 완료: {selected.augmentName}"); 

        
        //현재 선택 종료 
        isChoosingAugment = false;
        currentOfferedAugments.Clear();
        rerollHistory.Clear();

        OnAugmentSelectionClosed?.Invoke();

        TryprocessNextAugmentEvent(); //Queue 에 남은 다음 증강 이벤트 있으면 불러와서 처리하기
    }
    public void RerollSlot(int slotIndex)
    {
        if (!isChoosingAugment)
        {
            return;
        }
        if(slotIndex < 0 || slotIndex >= currentOfferedAugments.Count)
        {
            return;
        }

        if(slotIndex >= rerolledThisRound.Length)
        {
            return;
        }
        //슬롯 당 1회 리롤제한하기
        if (rerolledThisRound[slotIndex])
        {
            Debug.Log("이 슬롯은 이미 리롤을 사용했습니다.");
            return;
        }
        //현재 슬롯 카드 임시제거
        AugmentData oldData = currentOfferedAugments[slotIndex]; 
        currentOfferedAugments.RemoveAt(slotIndex);
        //현재 상태를 stack에 저장 
        List<AugmentData> snapshot = new List<AugmentData>(currentOfferedAugments);
        rerollHistory.Push(snapshot);

        List<AugmentData> candidates = BuildCandidateList(currentOfferTier, currentOfferedAugments);

        for(int i = candidates.Count -1; i >= 0; i--)
        {
            if(candidates[i] != null && candidates[i].augmentId == oldData.augmentId)
            {
                candidates.RemoveAt(i);
                //후보군 리스트에서 제거 / 리롤 전 카드 id 가 oldData 랑 같은지 비교하고 같으면 제거
            }
        }
        
        // 후보 카드 없으면 원래 카드 복구
        if(candidates.Count <= 0)
        {
            currentOfferedAugments.Insert(slotIndex, oldData);
            Debug.Log("리롤 가능한 후보가 없습니다.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, candidates.Count); //리롤 하면 후보군 카드에서 랜덤으로 1장 선택
        AugmentData newData = candidates[randomIndex];

        //원래 자리로 새 카드 삽입
        currentOfferedAugments.Insert(slotIndex, newData);

        //리롤 사용 처리
        rerolledThisRound[slotIndex] = true;

        Debug.Log($"리롤 완료: 슬롯 {slotIndex} / {oldData.augmentName} -> {newData.augmentName}");

        
    }

    public bool HasUsedRerollOnSlot(int slotIndex) //리롤 사용한 슬롯이지 확인하기
    {
        if (slotIndex < 0 || slotIndex >= rerolledThisRound.Length)
        {
            return false;
        }

        return rerolledThisRound[slotIndex];
    }
}
