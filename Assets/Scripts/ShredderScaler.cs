using UnityEngine;

public class ShredderScaler : MonoBehaviour
{
    [SerializeField] private bool _isLR;
    [SerializeField] private float _offset = 2f;

    private BoxCollider2D[] _collider2D;
    private Camera _camera;

    private void Awake()
    {
        _collider2D = GetComponents<BoxCollider2D>();
        _camera = Camera.main;
    }

    private void Start()
    {
        float halfHeight = _camera.orthographicSize;
        float halfWidth = _camera.aspect * halfHeight;

        if (_isLR)
        {
            _collider2D[0].offset = new(halfWidth+_offset, 0);
            _collider2D[1].offset = new(-halfWidth - _offset, 0);
        }
        else
        {
            foreach (BoxCollider2D collider in _collider2D)
                collider.size = new(halfWidth * 2 + _offset, collider.size.y);
        }
    }
}
