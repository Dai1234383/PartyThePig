using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialCtrl : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private RectTransform _startPos;
    [SerializeField] private RectTransform _endPos;
    [SerializeField] private float _actionTime = 0.5f;
    [SerializeField] private Button _selectButton; // 選択させたいボタン

    private RectTransform _panelRect;

    private void Awake()
    {
        if (_tutorialPanel != null)
            _panelRect = _tutorialPanel.GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        if (_tutorialPanel == null || _panelRect == null) return;

        // 初期状態
        _panelRect.anchoredPosition = _startPos.anchoredPosition;
        _panelRect.localScale = Vector3.zero;
        _selectButton.gameObject.SetActive(false);

        // アニメーション
        Sequence seq = DOTween.Sequence();
        seq.Join(_panelRect.DOAnchorPos(_endPos.anchoredPosition, _actionTime).SetEase(Ease.Linear));
        seq.Join(_panelRect.DOScale(Vector3.one, _actionTime).SetEase(Ease.Linear));

        seq.OnComplete(async () =>
        {
            _selectButton.gameObject.SetActive(true);

            // ボタンを選択状態にする
            EventSystem.current.SetSelectedGameObject(_selectButton.gameObject);
        });
    }

    public void CloseTutorial()
    {
        if (_tutorialPanel == null || _panelRect == null) return;

        // ボタン非表示
        _selectButton.gameObject.SetActive(false);

        // パネルを元に戻すアニメーション
        Sequence seq = DOTween.Sequence();
        seq.Join(_panelRect.DOAnchorPos(_startPos.anchoredPosition, _actionTime).SetEase(Ease.Linear));
        seq.Join(_panelRect.DOScale(Vector3.zero, _actionTime).SetEase(Ease.Linear));

        seq.OnComplete(() =>
        {
            _tutorialPanel.SetActive(false);
        });
    }
}
