using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void OnClickStart()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonSFX();
        }

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.StartGame();
        }
    }

    public void OnClickExit()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonSFX();
        }

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.QuitGame();
        }
    }
}
