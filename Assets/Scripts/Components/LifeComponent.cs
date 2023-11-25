using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    #region Variables
    [SerializeField] protected float _maxLife;
    protected float _currentLife;
    protected bool _isDead;
    protected enum AttackTypes { HighAttack, LowAttack }
    #endregion

    #region Unity methods
    protected virtual void Start()
    {
        _currentLife = _maxLife;
    }
    #endregion
    protected virtual void ReceiveHit(float amount, AttackTypes attackType = AttackTypes.HighAttack)
    {
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
        }
    }
}
