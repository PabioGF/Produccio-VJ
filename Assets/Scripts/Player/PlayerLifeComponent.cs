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
        // Checks if the player has dodged the hit
        if (_playerCombat.IsDodging)
        {
            if ((attackType == AttackTypes.HighAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.LowDodge) ||
            (attackType == AttackTypes.LowAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.HighDodge) ||
            attackType == AttackTypes.DefaultAttack)
            {
                // If it is dodged, doesn't take damage and the OnDodge function is called
                _playerCombat.OnDodge();
                return;
            }
        }

        // If the player is invulnerable returns
        if (_playerCombat.IsInvulnerable) return;

        Debug.Log("Hit");
        _hitStopController.StopTime(0f, _hitStopDuration);

        // If the player has a shield, removes it instead of taking the damage and stops
        if (_playerController.HasItem(InventoryItem.ItemType.Shield))
        {
            Debug.Log("Shield");
            _playerController.RemoveItem(InventoryItem.ItemType.Shield);
            return;
        }

        // Recieves the damage of the hit, updates the UI and checks if the player is dead
        _currentLife -= amount;
        UIController.Instance.SetLife(_currentLife);
        if (_currentLife <= 0) _isDead = true;

        // Shows the death screen if the player is dead
        if (_isDead)
        {
            PlayerController playerController = _parent.GetComponent<PlayerController>();
            playerController.Die();
            UIController.Instance.ShowDeathScreen();
        }

        // Visually shows the player has been hit
        StartCoroutine(_playerCombat.HitVisualFeedback());
    }

    /// <summary>
    /// Heals the player and updates the UI
    /// </summary>
    /// <param name="healingPoints">Amount to heal</param>
    public void Heal(int healingPoints)
    {
        _currentLife += healingPoints;
        UIController.Instance.SetLife(_currentLife);
    }
}