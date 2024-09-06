using System;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _offsetXLimit = 2f;
    [Header("Bullet")]
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Vector3 _spawnBullet = new(0, 1.2f, 0);
    [SerializeField] private float _reloadTime = 1f;

    private Camera _camera;
    private float _xLimit;

    private float _elapsedTime = 0f;

    public event Action Died;
    public event Action DestroedMeteor;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        float halfHeight = _camera.orthographicSize;
        _xLimit = _camera.aspect * halfHeight - _offsetXLimit;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        transform.Translate(_joystick.Horizontal * _speed * Time.deltaTime, 0f, 0f);

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -_xLimit, _xLimit);
        transform.position = position;

        if (_elapsedTime > _reloadTime)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition += _spawnBullet;

            Bullet bullet = Instantiate(_bullet, spawnPosition, Quaternion.identity);
            bullet.Killed += AddScore;

            _elapsedTime = 0f;
        }
    }

    private void AddScore(Bullet bullet ,bool isKilled)
    {
        if (isKilled)
            DestroedMeteor?.Invoke();

        bullet.Killed -= AddScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Meteor meteor))
            Died?.Invoke();
    }
}
