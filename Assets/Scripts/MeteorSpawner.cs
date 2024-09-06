using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private Meteor _prefab;
    [SerializeField] private float _minSpawnDelay = 1f;
    [SerializeField] private float _maxSpawnDelay = 3f;
    [SerializeField] private float _offsetSpawn = 2f;

    private float _spawnXLimit;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        float halfHeight = _camera.orthographicSize;
        _spawnXLimit = _camera.aspect * halfHeight - _offsetSpawn;

        Spawn();
    }

    private void Spawn()
    {
        float positionX = Random.Range(-_spawnXLimit, _spawnXLimit);
        Vector3 spawnPosition = transform.position + new Vector3(positionX, 0, 0);

        Instantiate(_prefab, spawnPosition, Quaternion.identity);

        Invoke(nameof(Spawn), Random.Range(_minSpawnDelay, _maxSpawnDelay));
    }
}
