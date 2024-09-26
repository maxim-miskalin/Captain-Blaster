using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _offsetXLimit = 1f;
    [Header("Bullet")]
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Vector3 _spawnBullet = new(0, 1.2f, 0);
    [SerializeField] private float _reloadTime = 1f;
    [Header("Pool")]
    [SerializeField] private int _maxContainsInactive = 10;

    private List<Bullet> _bulletsActive = new();
    private List<Bullet> _bulletsInactive = new();

    private Camera _camera;
    private float _xLimit;
    private Vector3 _startPosition;

    private float _elapsedTime = 0f;

    public event Action Died;
    public event Action DestroedMeteor;

    private void Awake()
    {
        _camera = Camera.main;
        _startPosition = transform.position;
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

            Bullet bullet = GetBullet();
            bullet.transform.position = spawnPosition;

            _elapsedTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Meteor meteor))
            Died?.Invoke();
    }

    public void ResetPool()
    {
        if (_bulletsActive.Count > 0)
            foreach (Bullet bullet in _bulletsActive.ToList())
                Release(bullet);

        transform.position = _startPosition;
    }

    private void AddScore(Bullet bullet, bool isKilled)
    {
        if (isKilled)
            DestroedMeteor?.Invoke();

        Release(bullet);
    }

    private Bullet Create()
    {
        Bullet bullet = Instantiate(_bullet);
        bullet.gameObject.SetActive(false);
        bullet.Killed += AddScore;
        _bulletsInactive.Add(bullet);

        return bullet;
    }

    private Bullet GetBullet()
    {
        Bullet bullet;

        if (_bulletsInactive.Count == 0)
        {
            bullet = Create();
            _bulletsActive.Add(bullet);
        }
        else
        {
            bullet = _bulletsInactive[_bulletsInactive.Count - 1];
            _bulletsInactive.Remove(bullet);
            _bulletsActive.Add(bullet);
        }

        bullet.gameObject.SetActive(true);
        bullet.StartMoving();

        return bullet;
    }

    private void Release(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);

        if (_bulletsActive.Contains(bullet))
            _bulletsActive.Remove(bullet);

        if (_bulletsInactive.Count >= _maxContainsInactive)
            DestroyBullet(bullet);
        else
            _bulletsInactive.Add(bullet);
    }

    private void DestroyBullet(Bullet bullet)
    {
        bullet.Killed -= AddScore;
        Destroy(bullet.gameObject);
    }
}
