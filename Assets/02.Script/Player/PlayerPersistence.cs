using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerPersistence : MonoBehaviour
{
    public static PlayerPersistence Instance;

    private void Awake()
    {
        // 이미 플레이어가 있으면 중복 제거
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬이 바뀌어도 플레이어 유지
        DontDestroyOnLoad(gameObject);

        // 씬이 로드될 때마다 실행될 함수 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬에서 PlayerSpawnPoint 찾기
        GameObject spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint");

        // 스폰 포인트가 있으면 플레이어 위치 이동
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
