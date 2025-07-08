using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankThePigStageCtrl : MonoBehaviour
{
    [SerializeField] private GameObject[] _stagePrefabs; // ステージのプレハブを複数登録

    private void Start()
    {
        if (_stagePrefabs.Length == 0)
        {
            Debug.LogWarning("ステージのPrefabが設定されていません！");
            return;
        }

        // ランダムで1つ選んで生成
        int randomIndex = Random.Range(0, _stagePrefabs.Length);
        Instantiate(_stagePrefabs[randomIndex], Vector3.zero, Quaternion.identity);
    }
}

