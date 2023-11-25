using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeComponent : LifeComponent
{
    #region Variables
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private float _respawnDelay;
    #endregion

    protected override void ReceiveHit(float amount, AttackTypes attackType)
    {
        if (attackType == AttackTypes.HighAttack && _playerCombat.IsDodgingUp() || attackType == AttackTypes.LowAttack && _playerCombat.IsDodgingDown()) return;

        base.ReceiveHit(amount);
        Invoke(nameof(Respawn), _respawnDelay);
    }

    private void Respawn()
    {
        transform.position = new Vector3(-5, 0, 0);
        gameObject.SetActive(true);
    }
}