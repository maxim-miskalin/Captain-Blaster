using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private Meteor _meteor;
    [SerializeField] private float _minSpawnDelay = 1f;
    [SerializeField] private float _maxSpawnDelay = 3f;
    [SerializeField] private float _offsetSpawn = 2f;
    [Header("Pool")]
    [SerializeField] private int _maxContainsInactive = 10;
    [SerializeField] private float _minScaleMeteor = 0.8f;

    private List<Meteor> _meteorsActive = new();
    private List<Meteor> _meteorsInactive = new();

    private bool _isWork = true;

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

        StartCoroutine(SpawnMeteor());
    }

    public void ResetPool()
    {
        if (_meteorsActive.Count > 0)
            foreach(Meteor meteor in _meteorsActive.ToList())
                Release(meteor);
    }

    private IEnumerator SpawnMeteor()
    {
        while (_isWork)
        {
            WaitForSeconds wait = new(Random.Range(_minSpawnDelay, _maxSpawnDelay));

            Meteor meteor = GetMeteor();
            meteor.transform.position = GetMeteorPosition();

            yield return wait;
        }
    }

    private Meteor GetMeteor()
    {
        Meteor meteor;

        if (_meteorsInactive.Count == 0)
        {
            meteor = Create();
            _meteorsActive.Add(meteor);
        }
        else
        {
            meteor = _meteorsInactive[_meteorsInactive.Count - 1];
            _meteorsInactive.Remove(meteor);
            _meteorsActive.Add(meteor);
        }

        meteor.gameObject.SetActive(true);
        meteor.StartMoving();

        return meteor;
    }

    private void SpawnChildMeteor(Vector3 parentPosition, int quantity)
    {
        for (int i = 0; i <= quantity; i++)
        {
            Vector3 spawnPosition = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Meteor meteor = GetMeteor();
            meteor.transform.position = parentPosition - spawnPosition;

            if (quantity > 1)
                meteor.transform.localScale /= quantity;
            else
                meteor.transform.localScale /= 2;

            if (meteor.transform.localScale.x < _minScaleMeteor)
                meteor.transform.localScale = Vector3.one * _minScaleMeteor;
        }
    }

    private Meteor Create()
    {
        Meteor meteor = GameObject.Instantiate(_meteor);
        meteor.transform.SetParent(transform);
        meteor.gameObject.SetActive(false);

        meteor.Died += Release;
        meteor.SpawnChild += SpawnChildMeteor;

        return meteor;
    }

    private Vector3 GetMeteorPosition()
    {
        float positionX = Random.Range(-_spawnXLimit, _spawnXLimit);

        return transform.position + new Vector3(positionX, 0, 0);
    }

    private void Release(Meteor meteor)
    {
        meteor.gameObject.SetActive(false);

        if (_meteorsActive.Contains(meteor))
            _meteorsActive.Remove(meteor);

        if (_meteorsInactive.Count >= _maxContainsInactive)
            DestroyMeteor(meteor);
        else
            _meteorsInactive.Add(meteor);
    }

    private void DestroyMeteor(Meteor meteor)
    {
        meteor.Died -= Release;
        meteor.SpawnChild -= SpawnChildMeteor;

        Destroy(meteor.gameObject);
    }
}
