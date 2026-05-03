using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    [Header("이 씬에서 재생할 BGM")]
    [SerializeField] private AudioClip sceneBGM;
    // 이 씬에서 틀고 싶은 BGM 파일을 넣는 곳

    [Header("BGM 없이 시작할지")]
    [SerializeField] private bool stopBGMInstead = false;
    // true면 BGM을 재생하지 않고 기존 BGM을 멈춤
    // 예: 조용한 ResultScene, 연출 씬 등에 사용 가능

    private void Start()
    {
        // SoundManager가 없으면 실행 안 함
        if (SoundManager.Instance == null)
        {
            return;
        }

        // BGM을 끄고 싶은 씬이면
        if (stopBGMInstead)
        {
            SoundManager.Instance.StopBGM();
            return;
        }

        // 이 씬의 BGM을 SoundManager에게 재생 요청
        SoundManager.Instance.PlayBGM(sceneBGM);
    }
}