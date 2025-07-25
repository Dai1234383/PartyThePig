using DG.Tweening;
using UnityEngine;

public class ShootThePigObjMover : MonoBehaviour
{
    private float _speed;
    private float _direction;
    private float _targetX;

    public void Initialize(float direction, float speed, float targetX)
    {
        _direction = direction;
        _speed = speed;
        _targetX = targetX;

        PlayRandomMovePattern();
    }

    private void PlayRandomMovePattern()
    {
        float distance = Mathf.Abs(_targetX - transform.position.x);
        float duration = distance / _speed;

        int pattern = Random.Range(0, 4); // 0:直線, 1:波状上, 2:波状下, 3:往復

        switch (pattern)
        {
            case 0: // 直線
                transform.DOMoveX(_targetX, duration)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject)
                    .OnComplete(() => Destroy(gameObject));
                break;

            case 1: // 波状（上にゆれる）
                Sequence waveSeqUp = DOTween.Sequence();
                waveSeqUp.Append(transform.DOMoveX(_targetX, duration).SetEase(Ease.Linear));
                waveSeqUp.Join(transform.DOLocalMoveY(transform.position.y + 2f, duration / 4)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .SetLink(gameObject));
                waveSeqUp.OnComplete(() => Destroy(gameObject));
                break;

            case 2: // 波状（下にゆれる）
                Sequence waveSeqDown = DOTween.Sequence();
                waveSeqDown.Append(transform.DOMoveX(_targetX, duration).SetEase(Ease.Linear));
                waveSeqDown.Join(transform.DOLocalMoveY(transform.position.y - 2f, duration / 4)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .SetLink(gameObject));
                waveSeqDown.OnComplete(() => Destroy(gameObject));
                break;

            case 3: // 往復 + 一時停止して戻る
                float midpointX = (transform.position.x + _targetX) / 2f;
                float halfDuration = duration / 2f;
                float pauseTime = 1f; // ← 停止時間（秒）

                Sequence returnSeq = DOTween.Sequence();
                returnSeq.Append(transform.DOMoveX(midpointX, halfDuration).SetEase(Ease.OutQuad).SetLink(gameObject));
                returnSeq.AppendInterval(pauseTime);
                returnSeq.Append(transform.DOMoveX(transform.position.x, halfDuration).SetEase(Ease.InQuad).SetLink(gameObject));
                returnSeq.OnComplete(() => Destroy(gameObject));
                break;
        }
    }
}
