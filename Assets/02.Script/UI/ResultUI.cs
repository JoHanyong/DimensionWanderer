using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("결과 텍스트")]
    [SerializeField] private TMP_Text resultText;
    // CLEAR 또는 GAME OVER를 보여줄 텍스트

    [Header("배경 이미지")]
    [SerializeField] private Image backgroundImage;
    // 결과에 따라 바뀔 배경 이미지

    [Header("클리어 배경")]
    [SerializeField] private Sprite clearBackground;
    // 클리어했을 때 보여줄 배경

    [Header("게임오버 배경")]
    [SerializeField] private Sprite gameOverBackground;
    // 게임오버일 때 보여줄 배경

    [Header("재시작할 씬 이름")]
    [SerializeField] private string retrySceneName = "Gamestage_1";
    // Retry 버튼을 눌렀을 때 이동할 씬

    [Header("타이틀 씬 이름")]
    [SerializeField] private string titleSceneName = "Start";
    // Title 버튼을 눌렀을 때 이동할 씬

    private void Start()
    {
        // 게임 클리어 상태라면
        if (GameResultData.isClear)
        {
            if (resultText != null)
            {
                resultText.text = "CLEAR!";
            }

            if (backgroundImage != null)
            {
                backgroundImage.sprite = clearBackground;
            }
        }
        // 게임 오버 상태라면
        else
        {
            if (resultText != null)
            {
                resultText.text = "GAME OVER";
            }

            if (backgroundImage != null)
            {
                backgroundImage.sprite = gameOverBackground;
            }
        }
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1f;

        GameResultData.isClear = false;

        
        

        GameObject oldplayer = GameObject.FindGameObjectWithTag("Player");

        if (oldplayer != null)
        {
            Destroy(oldplayer);
        }

        
        SceneManager.LoadScene(retrySceneName, LoadSceneMode.Single);
    }

    public void OnClickTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName, LoadSceneMode.Single);
    }
}
