using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("BGM音源")]
    public AudioSource audioSource;

    [Header("効果音（共通）")]
    public AudioSource sfxSource;
    public AudioClip sceneChangeSE;

    [Header("シーンごとのBGM")]
    public AudioClip titleBGM;
    public AudioClip homeBGM;
    public AudioClip jumpThePigBGM;
    public AudioClip escapeThePigBGM;
    public AudioClip moneyThePigBGM;
    public AudioClip shootThePigBGM;
    public AudioClip swingThePigBGM;
    public AudioClip tankThePigBGM;
    public AudioClip resultBGM;

    private Dictionary<string, AudioClip> bgmMap;
    private bool isFirstLoad = true; // 起動時かどうかを判定するフラグ

    private void Awake()
    {
        // シングルトン化（重複生成防止）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いで保持
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource が未設定なら自動取得
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // BGMマッピング
        bgmMap = new Dictionary<string, AudioClip>
        {
            { "00Title", titleBGM },
            { "01Home", homeBGM },
            { "02JumpThePig", jumpThePigBGM },
            { "02EscapeThePig", escapeThePigBGM },
            { "02MoneyThePig", moneyThePigBGM },
            { "02ShootThePig", shootThePigBGM },
            { "02SwingThePig", swingThePigBGM },
            { "02TankThePig", tankThePigBGM },
            { "03Result", resultBGM }
        };

        // シーン切り替えを監視
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 起動時（最初のシーン）だけ効果音をスキップ
        if (!isFirstLoad)
        {
            PlaySceneChangeSE();
        }
        else
        {
            isFirstLoad = false;
        }

        PlayBGMForScene(scene.name);
    }

    private void PlaySceneChangeSE()
    {
        if (sfxSource != null && sceneChangeSE != null)
        {
            sfxSource.PlayOneShot(sceneChangeSE);
        }
    }

    private void PlayBGMForScene(string sceneName)
    {
        if (bgmMap.TryGetValue(sceneName, out AudioClip clip))
        {
            if (audioSource.clip != clip)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"BGMが登録されていないシーン: {sceneName}");
        }
    }

    /// <summary>
    /// BGMのミュートを切り替える
    /// </summary>
    public void ToggleMute()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
        }
    }

    /// <summary>
    /// 現在のミュート状態を取得する
    /// </summary>
    /// <returns>ミュートならtrue、それ以外はfalse</returns>
    public bool IsMuted()
    {
        return audioSource != null && audioSource.mute;
    }
}
