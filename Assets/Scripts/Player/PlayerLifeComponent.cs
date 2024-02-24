using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerLifeComponent : MonoBehaviour
{
    #region Variables
    [Header("Specific fields")]
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HitStopController _hitStopController;
    [SerializeField] private float _hitStopDuration;

    [SerializeField] protected GameObject _parent;
    [SerializeField] protected float _maxLife;

    protected float _currentLife;
    protected bool _isDead;

    public enum AttackTypes
    {
        DefaultAttack = 0,
        HighAttack = 1,
        LowAttack = 2
    }
    #endregion

    private void Start()
    {
        _currentLife = _maxLife;
        UIController.Instance.SetLife(_currentLife);
    }

    public void ReceiveHit(float amount, AttackTypes attackType)
    {
        if (_playerCombat.IsDodging)
        {
            if ((attackType == AttackTypes.HighAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.LowDodge) ||
            (attackType == AttackTypes.LowAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.HighDodge) ||
            attackType == AttackTypes.DefaultAttack)
            {
                _playerCombat.OnDodge();
                return;
            }
        }

        if (_playerCombat.IsInvulnerable) return;

        Debug.Log("Hit");
        _hitStopController.StopTime(0f, _hitStopDuration);

        if (_playerController.HasItem(InventoryItem.ItemType.Shield))
        {
            Debug.Log("Shield");
            _playerController.RemoveItem(InventoryItem.ItemType.Shield);
            return;
        }

        _currentLife -= amount;
        if (_currentLife <= 0) _isDead = true;

        UIController.Instance.SetLife(_currentLife);
        if (_isDead)
        {
            PlayerController playerController = _parent.GetComponent<PlayerController>();
            playerController.Die();
            UIController.Instance.ShowDeathScreen();
        }

        StartCoroutine(_playerCombat.ReceiveHit());
    }
}