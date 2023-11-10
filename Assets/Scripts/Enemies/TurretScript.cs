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

    private bool _attackMode;
    private bool _upperBullet;
    #endregion

    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
        _attackMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    #endregion

    private void Shoot()
    {
        if (_attackMode) InvokeRepeating(nameof(SpawnBullet), 0, _fireRate);
    }

    private void SpawnBullet()
    {
        GameObject bullet = Random.Range(0, 1) > 0.1f ? _highBullet : _lowBullet;
        Instantiate(bullet, transform.position, transform.rotation, transform);
    }
}
