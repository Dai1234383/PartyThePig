using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MoneyThePigBom : MonoBehaviour
{
    [SerializeField] private float _maxBomAreaSize;     // 爆発範囲
    [SerializeField] private int _awaitTime;            // 爆発までの猶予
    [SerializeField] private float _bomberTime = 0.5f;  // 爆発にかかる時間
    [SerializeField] private TextMeshProUGUI _countText;    // カウントダウンテキスト
    [SerializeField] private GameObject _bomArea;           // 爆発範囲のオブジェクト

    [Header("効果音")]
    [SerializeField] private AudioClip _explosionSE;         // 爆発効果音
    [SerializeField] private AudioSource _audioSource;       // 効果音を鳴らすAudioSource

    private void FixedUpdate()
    {
        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.OVER)
        {
            Destroy(gameObject);
        }
    }

    private async void OnEnable()
    {
        var token = this.GetCancellationTokenOnDestroy();

        try
        {
            // カウントダウン
            for (int i = _awaitTime; i >= 0; i--)
            {
                if (_countText != null)
                    _countText.text = i.ToString();

                if (i == 0)
                {
                    Bomber(); // 爆発
                }

                await UniTask.Delay(1000, cancellationToken: token);
            }
        }
        catch (System.OperationCanceledException)
        {
            // オブジェクトが破棄されたため中断
        }
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    private void Bomber()
    {
        if (_bomArea == null) return;

        // 効果音再生
        if (_explosionSE != null)
        {
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(_explosionSE);
            }
            else
            {
                // AudioSource がなければその場で鳴らす
                AudioSource.PlayClipAtPoint(_explosionSE, transform.position);
            }
        }

        _bomArea.transform.DOScale(new Vector2(_maxBomAreaSize, _maxBomAreaSize), _bomberTime)
            .SetLink(gameObject)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }
}
