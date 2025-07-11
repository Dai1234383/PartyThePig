using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [Header("�������Ȃ� true")]
    public bool isInvisible = false;

    public void ActivateInvisibility(float duration)
    {
        if (!isInvisible)       // �A�Ŗh�~
            StartCoroutine(InvisibilityCoroutine(duration));
    }

    private IEnumerator InvisibilityCoroutine(float duration)
    {
        isInvisible = true;
        Debug.Log("�������X�^�[�g");

        // ���o���ʁF��������
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = new Color(1, 1, 1, 0.4f);

        yield return new WaitForSeconds(duration);

        // ����
        isInvisible = false;
        Debug.Log("�������I��");
        if (sr) sr.color = new Color(1, 1, 1, 1);
    }
}
