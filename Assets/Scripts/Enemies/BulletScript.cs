using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDodge : MonoBehaviour
{
    [SerializeField] private bool _bulletType;
    [SerializeField] private float _lifeSpan;
    [SerializeField] private float _speed;


    private Rigidbody2D _rigidbody;
    private float _timer;
    private Vector2 _direction;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        Move();
        
        _timer = Time.deltaTime;

        if (_timer > _lifeSpan) Destroy(gameObject);
    }

    private void Move()
    {
        _rigidbody.velocity = _direction.normalized * _speed;
    }

    public bool GetAttackType()
    {
        return _bulletType;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
