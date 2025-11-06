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

        int pattern = Random.Range(0, 4); // 0:’¼ü, 1:”góã, 2:”gó‰º, 3:‰•œ

        switch (pattern)
        {
            case 0: // ’¼ü
                transform.DOMoveX(_targetX, duration)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject)
                    .OnComplete(() => Destroy(gameObject));
                break;

            case 1: // ”góiã‚É‚ä‚ê‚éj
                {
                    Sequence waveSeqUp = DOTween.Sequence();
                    Tween moveXTween = transform.DOMoveX(_targetX, duration)
                        .SetEase(Ease.Linear)
                        .SetLink(gameObject);

                    Tween waveY = transform.DOLocalMoveY(transform.position.y + 2f, duration / 4)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine)
                        .SetLink(gameObject);

                    waveSeqUp.Append(moveXTween);
                    waveSeqUp.Join(waveY);

                    // XˆÚ“®‚ªI‚í‚Á‚½‚ç”g“®’âŽ~ ¨ ”j‰ó
                    moveXTween.OnComplete(() =>
                    {
                        waveY.Kill(); // Y‚Ì—h‚êTween‚ð’âŽ~
                        Destroy(gameObject);
                    });
                }
                break;

            case 2: // ”gói‰º‚É‚ä‚ê‚éj
                {
                    Sequence waveSeqDown = DOTween.Sequence();
                    Tween moveXTween = transform.DOMoveX(_targetX, duration)
                        .SetEase(Ease.Linear)
                        .SetLink(gameObject);

                    Tween waveY = transform.DOLocalMoveY(transform.position.y - 2f, duration / 4)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine)
                        .SetLink(gameObject);

                    waveSeqDown.Append(moveXTween);
                    waveSeqDown.Join(waveY);

                    moveXTween.OnComplete(() =>
                    {
                        waveY.Kill();
                        Destroy(gameObject);
                    });
                }
                break;

            case 3: // ‰•œ + ˆêŽž’âŽ~‚µ‚Ä–ß‚é
                float midpointX = (transform.position.x + _targetX) / 2f;
                float halfDuration = duration / 2f;
                float pauseTime = 1f; // © ’âŽ~ŽžŠÔi•bj

                Sequence returnSeq = DOTween.Sequence();
                returnSeq.Append(transform.DOMoveX(midpointX, halfDuration).SetEase(Ease.OutQuad).SetLink(gameObject));
                returnSeq.AppendInterval(pauseTime);
                returnSeq.Append(transform.DOMoveX(transform.position.x, halfDuration).SetEase(Ease.InQuad).SetLink(gameObject));
                returnSeq.OnComplete(() => Destroy(gameObject));
                break;
        }
    }
}
