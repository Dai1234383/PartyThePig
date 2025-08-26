using UnityEngine;
using UnityEngine.UI;

public class BGMMute : MonoBehaviour
{
    public GameObject bgmManagerObject; // BGMManagerオブジェクトをインスペクターで設定
    private AudioSource bgmAudioSource;

    // （オプション）ボタンの表示テキストを更新したい場合
    public Text buttonText;

    void Start()
    {
        if (bgmManagerObject != null)
        {
            bgmAudioSource = bgmManagerObject.GetComponent<AudioSource>();
        }

        UpdateButtonText(); // 初期状態の反映（オプション）
    }

    public void ToggleMute()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.mute = !bgmAudioSource.mute;
            UpdateButtonText();
        }
    }

    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = bgmAudioSource.mute ? "ミュート解除" : "ミュート";
        }
    }
}
