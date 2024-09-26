using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : MonoBehaviour, IPoolObject
{
    [SerializeField] private Meteor _prefab;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minRandomModifier = 0.5f;
    [SerializeField] private float _maxRandomModifier = 1.5f;
    [Header("Explode")]
    [SerializeField] private float _maxScaleToSpawnChild = 2f;
    [SerializeField] private int _minChildMeteor = 1;
    [SerializeField] private int _maxChildMeteor = 3;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    private Animator _animator;

    private float _normalSpeed;
    private float _normalScale;

    public event Action<Meteor> Died;
    public event Action<Vector3, int> SpawnChild;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();

        _normalSpeed = _speed;
        _normalScale = transform.localScale.x;
    }

    public void StartMoving()
    {
        MakeRandom();

        if (TryGetComponent(out Animator animator))
            animator.enabled = true;

        if (TryGetComponent(out Collider2D collider2D))
            collider2D.enabled = true;

        if (TryGetComponent(out Rigidbody2D rigidbody2D))
            rigidbody2D.simulated = true;

        _rigidbody2D.velocity = new(0, -_speed);
    }

    public void Return()
    {
        Died?.Invoke(this);

        _speed = _normalSpeed;
        transform.localScale = Vector3.one * _normalScale;

        _rigidbody2D.velocity = Vector2.zero;
    }

    private void MakeRandom()
    {
        float scaleModifier = Random.Range(_minRandomModifier, _maxRandomModifier);
        float speedModifier = Random.Range(_minRandomModifier, _maxRandomModifier);

        transform.localScale *= scaleModifier;
        _speed *= speedModifier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2D.simulated = false;
        _collider2D.enabled = false;
        _animator.SetTrigger(nameof(Died));

        if (transform.localScale.x > _maxScaleToSpawnChild)
        {
            int quantity = Random.Range(_minChildMeteor, _maxChildMeteor + 1);

            SpawnChild?.Invoke(transform.position, quantity);
        }
    }

    private void OnDestroy()
    {
        Died?.Invoke(this);
    }
}
