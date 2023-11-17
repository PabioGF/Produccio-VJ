using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeComponent : MonoBehaviour
{
    #region Variables
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private float _maxLife;
    private float _currentLife;
    private bool _isDead;
    #endregion

    #region Unity methods
    void Start()
    {
        _currentLife = _maxLife;
    }
    #endregion
    public void ReceiveHit(float amount)
    {
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            _isDead = true;
            Destroy(gameObject);
        }
    }
}
