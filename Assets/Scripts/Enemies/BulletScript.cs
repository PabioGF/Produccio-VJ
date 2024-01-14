using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _lifeSpan;
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private float _timer;
    private Vector2 _direction;
    #endregion

    #region Unity methods
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        _rigidbody.velocity = _direction.normalized * _speed;
        _timer = Time.deltaTime;
        if (_timer > _lifeSpan) Destroy(gameObject);
    }
    #endregion

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall") || collision.CompareTag("Roof"))
        {
            Destroy(gameObject);
        }
    }
}
