using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeComponent : LifeComponent
{
    #region Variables
    [Header("Specific fields")]
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HitStopController _hitStopController;
    [SerializeField] private float _hitStopDuration;
    #endregion

    public override void ReceiveHit(float amount, AttackTypes attackType)
    {
        if (_playerCombat.IsDodging)
        {
            if ((attackType == AttackTypes.HighAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.HighDodge) ||
            (attackType == AttackTypes.LowAttack && _playerCombat.GetDodgeType == PlayerCombat.DodgeType.LowDodge) ||
            attackType == AttackTypes.DefaultAttack)
            {
                _playerCombat.OnDodge();
                return;
            }
        }
        
        Debug.Log("Hit");
        _hitStopController.StopTime(0f, _hitStopDuration);

        if (_playerController.HasItem(InventoryItem.ItemType.Shield))
        {
            Debug.Log("Shield");
            _playerController.RemoveItem(InventoryItem.ItemType.Shield);
            return;
        }

        base.ReceiveHit(amount);
        if (_isDead)
        {
            PlayerController playerController = _parent.GetComponent<PlayerController>();
            playerController.Die();
            UIController.Instance.ShowDeathScreen();
        }  
    }
}