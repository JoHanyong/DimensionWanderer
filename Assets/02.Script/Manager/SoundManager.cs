using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM")]
    [SerializeField] private AudioClip mainBGM;

    [Header("Player SFX")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip skillSFX;
    [SerializeField] private AudioClip manaBallSFX;
    [SerializeField] private AudioClip parrySFX;
    [SerializeField] private AudioClip dashSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip hitSFX;

    [Header("Augment SFX")]
    [SerializeField] private AudioClip augmentOpenSFX;
    [SerializeField] private AudioClip augmentSelectSFX;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip enemyAttackSFX;
    [SerializeField] private AudioClip enemyDeathSFX;

    [Header("Etc SFX")]
    [SerializeField] private AudioClip levelUpSFX;
    [SerializeField] private AudioClip buttonSFX;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMainBGM();
    }
    public void PlayBGM(AudioClip bgmClip)
    {
        // BGM AudioSource가 없으면 실행 안 함
        if (bgmSource == null)
        {
            return;
        }

        // 재생할 BGM 파일이 없으면 실행 안 함
        if (bgmClip == null)
        {
            return;
        }

        // 지금 재생 중인 음악과 같은 음악이면 다시 재생하지 않음
        if (bgmSource.clip == bgmClip)
        {
            return;
        }

        // 재생할 BGM을 AudioSource에 넣음
        bgmSource.clip = bgmClip;

        // BGM은 반복 재생되도록 설정
        bgmSource.loop = true;

        // BGM 재생 시작
        bgmSource.Play();
    }
    public void StopBGM()
    {
        // BGM AudioSource가 없으면 실행 안 함
        if (bgmSource == null)
        {
            return;
        }

        // BGM 정지
        bgmSource.Stop();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("재생할 SFX 클립이 비어있음");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("SFX Source가 비어있음");
            return;
            
        }
        Debug.Log("SFX 재생됨 : " + clip.name);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMainBGM()
    {
        if (bgmSource == null || mainBGM == null)
        {
            return;
        }

        bgmSource.clip = mainBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayAttackSFX()
    {
        PlaySFX(attackSFX);
    }

    public void PlaySkillSFX()
    {
        PlaySFX(skillSFX);
    }

    public void PlayManaBallSFX()
    {
        PlaySFX(manaBallSFX);
    }

    public void PlayParrySFX()
    {
        PlaySFX(parrySFX);
    }

    public void PlayDashSFX()
    {
        PlaySFX(dashSFX);
    }

    public void PlayJumpSFX()
    {
        PlaySFX(jumpSFX);
    }

    public void PlayHitSFX()
    {
        PlaySFX(hitSFX);
    }

    public void PlayAugmentOpenSFX()
    {
        PlaySFX(augmentOpenSFX);
    }

    public void PlayAugmentSelectSFX()
    {
        PlaySFX(augmentSelectSFX);
    }

    public void PlayEnemyAttackSFX()
    {
        PlaySFX(enemyAttackSFX);
    }

    public void PlayEnemyDeathSFX()
    {
        PlaySFX(enemyDeathSFX);
    }

    public void PlayLevelUpSFX()
    {
        PlaySFX(levelUpSFX);
    }

    public void PlayButtonSFX()
    {
        PlaySFX(buttonSFX);
    }
}