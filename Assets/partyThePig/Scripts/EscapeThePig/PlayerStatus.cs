using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [Header("透明中なら true")]
    public bool isInvisible = false;

    public void ActivateInvisibility(float duration)
    {
        if (!isInvisible)       // 連打防止
            StartCoroutine(InvisibilityCoroutine(duration));
    }

    private IEnumerator InvisibilityCoroutine(float duration)
    {
        isInvisible = true;
        Debug.Log("透明化スタート");

        // 視覚効果：半透明に
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = new Color(1, 1, 1, 0.4f);

        yield return new WaitForSeconds(duration);

        // 解除
        isInvisible = false;
        Debug.Log("透明化終了");
        if (sr) sr.color = new Color(1, 1, 1, 1);
    }
}
