using UnityEngine;
using UnityEngine.SceneManagement;
// SceneManager를 사용하기 위해 필요

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    // 어디서든 접근하기 위한 싱글톤

    private void Awake()
    {
        // 이미 다른 SceneLoader가 있다면 중복 생성 방지
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬이 바뀌어도 이 오브젝트는 삭제되지 않도록 유지
        DontDestroyOnLoad(gameObject);
    }

    
    // 씬 이름으로 이동하는 함수
    
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        // Build Settings에 등록된 씬 이름으로 이동
    }

    
    // 씬 번호(index)로 이동하는 함수
    
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        // Build Settings에서 순서대로 번호가 매겨짐
        // 0 = TitleScene
        // 1 = GameScene_1
    }

    
    // 다음 씬으로 이동
    
    public void LoadNextScene()
    {
        // 현재 씬 번호 가져오기
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 다음 씬 번호 계산
        int nextSceneIndex = currentSceneIndex + 1;

        // 다음 씬이 존재하면 이동
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // 마지막 씬이면 로그 출력
            Debug.Log("다음 씬이 없습니다.");
        }
    }

    
    // 게임 시작 버튼용 함수
    
    public void StartGame()
    {
        // 1번 씬으로 이동 (GameScene_1)
        LoadSceneByIndex(1);
    }

    
    // 타이틀 씬으로 이동
    
    public void LoadTitle()
    {
        LoadSceneByIndex(0);
    }

    
    // 게임 종료
    
    public void QuitGame()
    {
        Application.Quit();
        // 빌드된 게임에서만 작동
    }
}
