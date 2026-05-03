using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private HealthPointManager playerHealth;

    [Header("씬 이동 대기 시간")]
    [SerializeField] private float clearDelay = 3f;
    [SerializeField] private float gameOverDelay = 2f;

    [Header("결과 씬 이름")]
    [SerializeField] private string resultSceneName = "END";

    private bool isGameEnd = false;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<HealthPointManager>();
        }
        // 플레이어가 죽었을 때 OnPlayerDead 함수가 실행되도록 연결
        if (playerHealth != null)
        {
            playerHealth.OnDead += OnPlayerDead;
        }
    }

    private void Update()
    {
        // 이미 게임이 끝났으면 몬스터 체크 안 함
        if (isGameEnd)
        {
            return;
        }

        // 매 프레임 몬스터가 남아있는지 확인
        CheckAllMonsterDead();
    }

    private void CheckAllMonsterDead()
    {
        // 현재 씬에 있는 모든 몬스터 찾기
        EnemyFSMController[] enemies =
             FindObjectsByType<EnemyFSMController>(FindObjectsSortMode.None);

        // 몬스터가 하나도 없으면 클리어
        if (enemies.Length <= 0)
        {
            isGameEnd = true;
            StartCoroutine(ClearRoutine());
        }
    }

    private void OnPlayerDead()
    {
        // 이미 클리어/게임오버 처리 중이면 중복 실행 방지
        if (isGameEnd)
        {
            return;
        }

        isGameEnd = true;

        Debug.Log("플레이어 사망 → 게임 오버");

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator ClearRoutine()
    {
        Debug.Log("몬스터 전멸");

        yield return new WaitForSeconds(clearDelay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("마지막 스테이지 클리어 → 결과 화면");

            GameResultData.isClear = true;
            SceneManager.LoadScene(resultSceneName);
        }
        else
        {
            Debug.Log("다음 스테이지 이동");

            SceneManager.LoadScene(nextIndex);
        }
    }

    private IEnumerator GameOverRoutine()
    {
        Debug.Log("게임 오버");

        GameResultData.isClear = false;

        yield return new WaitForSeconds(gameOverDelay);

        SceneManager.LoadScene(resultSceneName);
    }

    private void OnDestroy()
    {
        // 이벤트 연결 해제
        if (playerHealth != null)
        {
            playerHealth.OnDead -= OnPlayerDead;
        }
    }
}