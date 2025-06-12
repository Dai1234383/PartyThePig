using UnityEngine;

public class JumpThePigStageCtrl : MonoBehaviour
{
    [SerializeField] private GameObject[] _platformPrefabs;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _xRange = 6f;
    [SerializeField] private float _yInterval = 2f;
    [SerializeField] private int _initialPlatformCount = 10;
    [SerializeField] private float _spawnBufferHeight = 10f;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private float _riseSpeed;
    [SerializeField] private GameObject _moveObj;
    [SerializeField] private float _cameraFollowSpeed = 2f;

    [SerializeField] private float _platformHeightOffset = 0.5f; // Å© Åö í«â¡ÅFë´èÍÇÃçÇÇ≥ï‚ê≥

    private float _highestPlatformY;
    private float _cameraTargetY;

    private void Start()
    {
        for (int i = 0; i < _initialPlatformCount; i++)
        {
            SpawnPlatform(i * _yInterval);
        }

        _highestPlatformY = _initialPlatformCount * _yInterval;
        _cameraTargetY = _cameraTransform.position.y;
    }

    private void Update()
    {
        if (JumpThePigGameStateManager.Instance.GameState == JumpThePigGameStateManager.GameStateName.GAME)
        {
            _moveObj.transform.position += Vector3.up * _riseSpeed * Time.deltaTime;
        }

        _cameraTargetY = _moveObj.transform.position.y;
        Vector3 cameraPos = _cameraTransform.position;
        cameraPos.y = Mathf.Lerp(cameraPos.y, _cameraTargetY, Time.deltaTime * _cameraFollowSpeed);
        _cameraTransform.position = cameraPos;

        SpawnStage();
    }

    private void SpawnStage()
    {
        float targetY = _playerTransform.position.y + _spawnBufferHeight;

        while (_highestPlatformY < targetY)
        {
            SpawnPlatform(_highestPlatformY);
            _highestPlatformY += _yInterval;
        }
    }

    private void SpawnPlatform(float yPos)
    {
        float xPos = Random.Range(-_xRange, _xRange);
        GameObject prefab = _platformPrefabs[Random.Range(0, _platformPrefabs.Length)];
        Vector3 spawnPos = new Vector3(xPos, yPos + _platformHeightOffset, 0); // Å© Åö ï‚ê≥Çí«â¡
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
