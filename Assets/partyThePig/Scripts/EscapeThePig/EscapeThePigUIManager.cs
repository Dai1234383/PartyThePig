using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EscapeThePigUIManager : MonoBehaviour
{
    public static EscapeThePigUIManager Instance;

    [Header("ゲーム進行UI")]
    [SerializeField] private TextMeshProUGUI _gameText;
    [SerializeField] private Button _toResultButton;
    [SerializeField] private Button _restartButton;

    [Header("プレイヤーごとのアイテムアイコンUI")]
    [SerializeField] private List<Image> _playerItemIcons = new(); // Image型に変更！

    [SerializeField] private Sprite _defaultIcon; // アイテムなし時のデフォルト（透明画像など）

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        _gameText.text = "3";
        await UniTask.Delay(1000);
        _gameText.text = "2";
        await UniTask.Delay(1000);
        _gameText.text = "1";
        await UniTask.Delay(1000);
        _gameText.text = "GameStart!!";
        await UniTask.Delay(1000);
        _gameText.gameObject.SetActive(false);

        SetupItemUIListeners();
    }

    private void SetupItemUIListeners()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length && i < _playerItemIcons.Count; i++)
        {
            var itemSystem = players[i].GetComponent<EscapeThePigPlayerItemSystem>();
            int index = i;

            if (itemSystem != null)
            {
                UpdateItemIcon(index, itemSystem.GetHeldItem());

                itemSystem.OnItemChanged.AddListener((item) =>
                {
                    UpdateItemIcon(index, item);
                });
            }
        }
    }

    private void UpdateItemIcon(int index, GameObject item)
    {
        if (index < 0 || index >= _playerItemIcons.Count) return;

        Image iconImage = _playerItemIcons[index];

        if (item != null)
        {
            var icon = item.GetComponent<EscapeThePigItemIcon>();
            if (icon != null && icon.itemIcon != null)
            {
                iconImage.sprite = icon.itemIcon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = _defaultIcon;
                iconImage.enabled = true;
            }
        }
        else
        {
            iconImage.sprite = _defaultIcon;
            iconImage.enabled = true;
        }
    }

    public void GameOverUI(string winnerName)
    {
        _toResultButton.gameObject.SetActive(true);
        _gameText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _gameText.text = winnerName + " is the WINNER";
    }
}
