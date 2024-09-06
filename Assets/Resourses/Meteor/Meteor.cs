using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private Meteor _prefab;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _minRandomModifier = 0.5f;
    [SerializeField] private float _maxRandomModifier = 1.5f;
    [Header("Explode")]
    [SerializeField] private float _maxScaleToSpawnChild = 2f;
    [SerializeField] private int _minChildMeteor = 1;
    [SerializeField] private int _maxChildMeteor = 4;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MakeRandom();
        _rigidbody2D.velocity = new(0, -_speed);
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
        if (transform.localScale.x > _maxScaleToSpawnChild)
        {
            List<Meteor> meteors = new();
            int quantity = Random.Range(_minChildMeteor, _maxChildMeteor);

            for (int i = 0; i < quantity; i++)
            {
                Vector3 spawnPosition = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Meteor meteor = Instantiate(_prefab, transform.position - spawnPosition, Quaternion.identity);

                if (quantity > 1)
                    meteor.transform.localScale /= quantity;
                else
                    meteor.transform.localScale /= 2;

                meteors.Add(meteor);
            }

            foreach (Meteor meteor in meteors)
            {
                meteor.GetComponent<Animator>().enabled = true;
                meteor.GetComponent<Collider2D>().enabled = true;
                meteor.GetComponent<Meteor>().enabled = true;
            }
        }
    }

    private void OnDestroy()
    {

    }
}
