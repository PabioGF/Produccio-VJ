using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDodge : MonoBehaviour
{
    [SerializeField] private bool _bulletType;
    [SerializeField] private float _lifeSpan;

    private Rigidbody2D rb;
    public float speed;
    public bool upAttack;
    private float _timer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        rb.velocity = Vector3.left * speed;
        _timer = Time.deltaTime;

        if (_timer > _lifeSpan) Destroy(gameObject);
    }

    public bool GetAttackType()
    {
        return _bulletType;
    }
}
