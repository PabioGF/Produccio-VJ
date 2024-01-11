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

    private IAController _controller;
    protected float _currentLife;
    protected bool _isDead;
    #endregion

    #region Unity methods
    protected virtual void Start()
    {
        _currentLife = _maxLife;
        _controller = GetComponent<IAController>();
    }
    #endregion
    public virtual void ReceiveHit(float amount)
    {
        _controller.GetHit();
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
        }
    }

    public void SendFlyingUp(float force)
    {
        _parent.GetComponent<Rigidbody2D>().AddForce(new(0, force) ,ForceMode2D.Impulse);
    }

    public void SendFlyingDown(float force)
    {
        _parent.GetComponent<Rigidbody2D>().AddForce(new(0, -force), ForceMode2D.Impulse);
    }
}
