using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _highBullet;
    [SerializeField] private GameObject _lowBullet;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _upperBulletProbability;
    [SerializeField] private GameObject _referencePoint;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _bulletSpeed;

    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private bool _upperBullet;
    private Vector2 _aimDirection;
    private bool _playerDetected;
    #endregion

    #region Unity methods
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        if (_playerDetected) {
            CalculateDirection();
        }
    }
    #endregion

    private void CalculateDirection()
    {
        _aimDirection = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
        _referencePoint.GetComponent<Rigidbody2D>().rotation = angle;        
    }

    private void SpawnBullet()
    {
        GameObject bullet = Random.Range(0f, 1f) > _upperBulletProbability ? _highBullet : _lowBullet;
        GameObject newBullet = Instantiate(bullet, _pointer.transform.position, _pointer.transform.rotation);
        newBullet.GetComponent<BulletScript>().SetDirection(_aimDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerDetected = true;
            InvokeRepeating(nameof(SpawnBullet), 0, _fireRate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerDetected = false;
            CancelInvoke(nameof(SpawnBullet));
        }
    }
}
