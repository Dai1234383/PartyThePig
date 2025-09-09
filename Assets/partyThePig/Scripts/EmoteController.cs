using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class EmoteController : MonoBehaviour
{
    [Header("エモートスプライト配列")]
    [SerializeField] private Sprite[] _emoteSprites;
    // 0 = RB, 1 = LB, 2 = RT, 3 = LT とする

    [Header("エモート設定")]
    [SerializeField] private float _duration = 0.3f;  // 出現/消滅アニメーション時間
    [SerializeField] private float _displayTime = 2f; // 表示時間
    [SerializeField] private Vector2 _offset = new Vector2(1f, 1f); // 右上オフセット

    private bool _isShowing = false; // 表示中フラグ

    // ========================
    // Inputから呼ばれるメソッド
    // ========================
    public void EmoteRB(InputAction.CallbackContext context)
    {
        if (context.performed) ShowEmote(0);
    }

    public void EmoteLB(InputAction.CallbackContext context)
    {
        if (context.performed) ShowEmote(1);
    }

    public void EmoteRT(InputAction.CallbackContext context)
    {
        if (context.performed) ShowEmote(2);
    }

    public void EmoteLT(InputAction.CallbackContext context)
    {
        if (context.performed) ShowEmote(3);
    }

    // ========================
    // エモート生成処理
    // ========================
    private void ShowEmote(int index)
    {
        if (_isShowing) return; // すでに表示中なら何もしない
        if (_emoteSprites == null || index < 0 || index >= _emoteSprites.Length) return;

        Sprite sprite = _emoteSprites[index];
        if (sprite == null) return;

        _isShowing = true;

        // GameObject生成
        GameObject emoteObj = new GameObject("Emote");
        emoteObj.transform.SetParent(transform);
        emoteObj.transform.localPosition = _offset; // 右上固定
        emoteObj.transform.localScale = Vector3.zero;

        // SpriteRenderer追加
        SpriteRenderer sr = emoteObj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 100; // 前面に表示

        // DOTweenアニメーション
        Sequence seq = DOTween.Sequence();
        seq.Append(emoteObj.transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutBack)) // 出現
           .AppendInterval(_displayTime) // 表示維持
           .Append(emoteObj.transform.DOScale(Vector3.zero, _duration).SetEase(Ease.InBack)) // 消滅
           .OnComplete(() =>
           {
               Destroy(emoteObj);
               _isShowing = false; // 表示終了
           });
    }
}
