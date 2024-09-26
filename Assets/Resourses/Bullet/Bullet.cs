using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody2D;

    public event Action<Bullet, bool> Killed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void StartMoving()
    {
        _rigidbody2D.velocity = new(0f, _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPoolObject _))
            Killed?.Invoke(this, true);
        else
            Killed?.Invoke(this, false);
    }

    public void Return()
    {
        Killed?.Invoke(this, false);
    }

    private void OnDestroy()
    {
        Killed?.Invoke(this, false);
    }
}
