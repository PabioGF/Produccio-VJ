using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeComponent : LifeComponent
{
    #region Variables
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private float _hitStopDuration;
    [SerializeField] private HitStopController _hitStopController;
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
        base.ReceiveHit(amount);
        if (_isDead)
            Invoke(nameof(Respawn), _respawnDelay);
        
    }

    private void Respawn()
    {
        _isDead = false;
        _parent.transform.position = new Vector3(-5, 0, 0);
        _currentLife = _maxLife;
        _parent.gameObject.SetActive(true);
    }
}