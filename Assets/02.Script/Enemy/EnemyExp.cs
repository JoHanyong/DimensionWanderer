using UnityEngine;

public class EnemyExp : MonoBehaviour
{
    [Header("경험치 보상")]
    [SerializeField] private int expReward = 1;

    [Header("참조")]
    [SerializeField] private HealthPointManager health; //몬스터 체력

    private bool hasGivenExp = false; //중복 지급 방지

    private void Awake()
    {
        if(health == null)
        {
            health = GetComponent<HealthPointManager>();
        }
    }
    private void Update()
    {
        if (hasGivenExp)
        {
            return;
        }
        if (health == null)
        {
            return;
        }

        if (health.IsDead)
        {
            GiveExpToPlayer();
            hasGivenExp = true; 
        } 
    }
    private void GiveExpToPlayer()
    {
        //player 태크를 가진 오브젝트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾기 못했습니다.");
            return;
        }

        PlayerLevelSystem levelSystem = player.GetComponent<PlayerLevelSystem>();

        if (levelSystem == null) 
        {
            Debug.LogWarning("PlayerLevelSystem 없음");
            return;
        }

        levelSystem.AddExp(expReward);
    }
}
