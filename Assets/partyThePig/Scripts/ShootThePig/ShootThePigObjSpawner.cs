using UnityEngine;

public class ShootThePigObjSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float spawnRate;
        public float moveSpeed = 5f;
    }

    [SerializeField] private SpawnData[] _spawnObjects;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnPoint = 10f;
    [SerializeField] private float _spawnRange = 4f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {

        if (ShootThePigGameStateManager.Instance.GameState != ShootThePigGameStateManager.GameStateName.GAME) return;

        SpawnData data = GetRandomSpawnData();
        if (data == null || data.prefab == null) return;

        bool spawnFromRight = Random.value > 0.5f;
        float x = spawnFromRight ? _spawnPoint : -_spawnPoint;
        float y = Random.Range(-_spawnRange, _spawnRange);
        Vector3 spawnPos = new Vector3(x, y, 0f);

        GameObject obj = Instantiate(data.prefab, spawnPos, Quaternion.identity);

        float direction = spawnFromRight ? -1f : 1f;
        float targetX = -x;

        obj.AddComponent<ShootThePigObjMover>().Initialize(direction, data.moveSpeed, targetX);
    }

    private SpawnData GetRandomSpawnData()
    {
        float total = 0f;
        foreach (var data in _spawnObjects) total += data.spawnRate;

        float rand = Random.Range(0f, total);
        float sum = 0f;
        foreach (var data in _spawnObjects)
        {
            sum += data.spawnRate;
            if (rand <= sum) return data;
        }

        return null;
    }
}
