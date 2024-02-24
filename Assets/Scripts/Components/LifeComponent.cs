using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LifeComponent : MonoBehaviour
{
    #region Variables
    [Header("Life component fields")]
    [SerializeField] protected GameObject _parent;
    [SerializeField] protected float _maxLife;

    protected float _currentLife;
    protected bool _isDead;
    #endregion

    #region Unity methods
    protected virtual void Start()
    {
        _currentLife = _maxLife;
    }
    #endregion
    public virtual void ReceiveHit(float amount)
    {
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
        }
    }

    public virtual void SendFlyingUp(float force)
    {
       
    }

    public virtual void SendFlyingDown(float force)
    {
       
    }

    public virtual void SendFlyingOutwards(float force)
    {

    }

    public bool IsDead => _isDead;
}
