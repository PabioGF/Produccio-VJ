using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _fireRate;
    [SerializeField] private Rigidbody2D _referencePoint;
    [SerializeField] private GameObject _bulletSpawnPoint;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _disableSound;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private Vector2 _aimDirection;
    private bool _playerDetected;
    private bool _isDisarmed;
    private AudioSource _audioSource;
    #endregion

    #region Unity methods
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (_playerDetected) {
            CalculateDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDisarmed) return;

        // If the player enters the trigger, starts shooting at it
        if (collision.CompareTag("Player"))
        {
            CalculateDirection();
            _playerDetected = true;
            InvokeRepeating(nameof(SpawnBullet), 0, _fireRate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isDisarmed) return;

        // If the player exits the trigger, stops shooting at it
        if (collision.CompareTag("Player"))
        {
            _playerDetected = false;
            CancelInvoke(nameof(SpawnBullet));
        }
    }
    #endregion

    /// <summary>
    /// Calculates the direction where the player is
    /// </summary>
    private void CalculateDirection()
    {
        _aimDirection = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
        _referencePoint.rotation = angle;        
    }

    /// <summary>
    /// Spawns a new bullet, with a 50% chance on each type
    /// </summary>
    private void SpawnBullet()
    {
        _animator.SetTrigger("Shoot");
        if (_shootSound != null)
        {
            _audioSource.PlayOneShot(_shootSound); 
        }
        GameObject newBullet = Instantiate(_bullet, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
        newBullet.GetComponent<BulletScript>().SetDirection(_aimDirection);
    }

    /// <summary>
    /// Stops the turret from acting
    /// </summary>
    public void DisarmTurret()
    {
        _isDisarmed = true;
        _playerDetected = false;
        CancelInvoke(nameof(SpawnBullet));
        _animator.SetTrigger("Disarm");

        if (_disableSound != null)
        {
            _audioSource.PlayOneShot(_disableSound);
        }
    }
}
