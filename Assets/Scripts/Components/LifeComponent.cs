using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    #region Variables
    [SerializeField] protected GameObject _parent;
    [SerializeField] protected float _maxLife;
    protected float _currentLife;
    protected bool _isDead;
    public enum AttackTypes { DefaultAttack, HighAttack, LowAttack }
    #endregion

    #region Unity methods
    protected virtual void Start()
    {
        _currentLife = _maxLife;
    }
    #endregion
    public virtual void ReceiveHit(float amount, AttackTypes attackType = AttackTypes.DefaultAttack)
    {
        _currentLife -= amount;
        Debug.Log("Aqui");

        if (_currentLife <= 0)
        {
            _isDead = true;
            _parent.SetActive(false);
        }
    }
}
