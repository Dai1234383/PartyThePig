using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MoneyThePigBom : MonoBehaviour
{
    [SerializeField] private float _maxBomAreaSize;     //爆発範囲
    [SerializeField] private int _awaitTime;            //爆発までの猶予
    [SerializeField] private float _bomberTime = 0.5f;  //爆発にかかる時間
    [SerializeField] private TextMeshProUGUI _countText;    //カウントダウンテキスト
    [SerializeField] private GameObject _bomArea;           //爆発範囲のオブジェクト

    private MoneyThePigPlayerCtrl _playerCtrl;

    private void FixedUpdate()
    {
        if(MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.OVER)
        {
            Destroy(gameObject);
        }
    }
    private async void OnEnable()
    {
        //カウントダウン
        for (int i = _awaitTime; i >= 0; i--)
        {
            _countText.text = i.ToString();
            if (i == 0)
            {
                Bomber();
            }
            await UniTask.Delay(1000);
        }
    }

    /// <summary>
    /// 爆発
    /// </summary>
    private void Bomber()
    {
        _bomArea.transform.DOScale(new Vector2(_maxBomAreaSize, _maxBomAreaSize), _bomberTime)
            .SetLink(gameObject)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }
}
