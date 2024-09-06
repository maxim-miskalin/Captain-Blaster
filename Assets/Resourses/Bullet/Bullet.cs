using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody2D;

    public event Action<Bullet ,bool> Killed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody2D.velocity = new(0f, _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Meteor meteor))
        {
            Destroy(collision.gameObject);
            Killed?.Invoke(this, true);
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Killed?.Invoke(this, false);
    }
}
