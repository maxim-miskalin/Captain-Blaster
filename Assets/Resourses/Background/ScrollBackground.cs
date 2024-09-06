using Unity.VisualScripting;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private float _upperYValue;
    [SerializeField] private float _lowerYValue;
    [SerializeField] private float _speed = 2f;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _upperYValue = _renderer.bounds.size.y;
        _lowerYValue = -_renderer.bounds.size.y;
    }

    private void FixedUpdate()
    {
        transform.Translate(0f, -_speed * Time.fixedDeltaTime, 0f);

        if (transform.position.y < _lowerYValue)
            transform.position = new(0f, _upperYValue, 0f);
    }
}