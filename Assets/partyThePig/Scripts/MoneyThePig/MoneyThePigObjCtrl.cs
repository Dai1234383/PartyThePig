using System.Collections;
using UnityEngine;

public class MoneyThePigObjCtrl : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject prefab;
        [Range(0f, 100f)]
        public float spawnRate; // 出現率（0〜100）
    }

    [Header("出現するオブジェクトと出現率の設定")]
    [SerializeField] private SpawnData[] _spawnObjects;

    [Header("出現間隔（秒）")]
    [SerializeField] private float _spawnInterval = 2f;

    [Header("出現範囲（XとZの±範囲）")]
    [SerializeField] private float _spawnRangeX = 10f;
    [SerializeField] private float _spawnRangeZ = 10f;

    [Header("出現Y座標")]
    [SerializeField] private float _spawnHeight = 0f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnRandomObject()
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;
        if (_spawnObjects.Length == 0) return;

        GameObject selectedPrefab = GetRandomPrefabByRate();
        if (selectedPrefab == null) return;

        float x = Random.Range(-_spawnRangeX, _spawnRangeX);
        float z = Random.Range(-_spawnRangeZ, _spawnRangeZ);
        Vector3 spawnPos = new Vector3(x, _spawnHeight, z);

        Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
    }

    private GameObject GetRandomPrefabByRate()
    {
        float totalRate = 0f;
        foreach (var data in _spawnObjects)
        {
            totalRate += data.spawnRate;
        }

        float randomValue = Random.Range(0, totalRate);
        float currentRate = 0f;

        foreach (var data in _spawnObjects)
        {
            currentRate += data.spawnRate;
            if (randomValue <= currentRate)
            {
                return data.prefab;
            }
        }

        return null;
    }
}
