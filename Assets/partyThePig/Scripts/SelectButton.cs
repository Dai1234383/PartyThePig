using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] Button _selectButton;

    private void OnEnable()
    {
        _selectButton.Select();
    }
}
