using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro用
using UnityEngine.UI; // UI関連（ImageやButton）

public class GameManager : MonoBehaviour
{
    // ▼ ミュートボタンに表示されるテキスト（任意）
    [SerializeField]
    private TextMeshProUGUI bgmButtonText;

    // ▼ ミュートボタンのImageコンポーネント（画像を切り替える）
    [SerializeField]
    private Image bgmButtonImage;

    // ▼ オン状態のスプライト画像
    [SerializeField]
    private Sprite bgmOnSprite;

    // ▼ オフ状態のスプライト画像
    [SerializeField]
    private Sprite bgmOffSprite;

    // 【追加】ゲームスタートボタン（UI）もここで取得したいなら設定可能
    // [SerializeField] private GameObject gameStartButtonGameObject;

    private void Start()
    {
        UpdateBGMButtonUI();
        ApplyAudioListenerState();
    }

    /// <summary>
    /// BGMのオン／オフを切り替える（ボタンから呼ばれる）
    /// </summary>
    public void ToggleBGM()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.ToggleMute();
            UpdateBGMButtonUI();

            // 【追加】ミュート状態に応じてAudioListenerの有効無効を切り替え
            ApplyAudioListenerState();
        }
        else
        {
            Debug.LogWarning("BGMManager.Instance が見つかりません！");
        }
    }

    /// <summary>
    /// AudioListenerのenabledをミュート状態に応じて切り替える
    /// </summary>
    private void ApplyAudioListenerState()
    {
        if (BGMManager.Instance == null) return;

        bool isMuted = BGMManager.Instance.IsMuted();

        AudioListener[] listeners = FindObjectsOfType<AudioListener>();

        foreach (var listener in listeners)
        {
            // AudioListenerコンポーネントのenabledだけ切り替える（GameObjectは触らない）
            listener.enabled = !isMuted;
        }
    }

    /// <summary>
    /// ボタンのテキストと画像を更新する
    /// </summary>
    private void UpdateBGMButtonUI()
    {
        if (BGMManager.Instance == null) return;

        bool isMuted = BGMManager.Instance.IsMuted();

        if (bgmButtonText != null)
        {
            bgmButtonText.text = isMuted ? "BGMオン" : "BGMオフ";
        }

        if (bgmButtonImage != null)
        {
            bgmButtonImage.sprite = isMuted ? bgmOffSprite : bgmOnSprite;
        }
    }

    // --- 以下はシーン遷移など既存コード（省略しません）---

    public void LoadTitleScene() => SceneManager.LoadScene("00Title");
    public void LoadHomeScene() => SceneManager.LoadScene("01Home");
    public void LoadJumpThePigScene() => SceneManager.LoadScene("02JumpThePig");
    public void LoadEscapeThePigScene() => SceneManager.LoadScene("02EscapeThePig");
    public void LoadMoneyThePigScene() => SceneManager.LoadScene("02MoneyThePig");
    public void LoadShootThePigScene() => SceneManager.LoadScene("02ShootThePig");
    public void LoadSwingThePigScene() => SceneManager.LoadScene("02SwingThePig");
    public void LoadTankThePigScene() => SceneManager.LoadScene("02TankThePig");
    public void LoadResultScene() => SceneManager.LoadScene("03Result");

    public void ReStartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
