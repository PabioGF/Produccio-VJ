using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeComponent : MonoBehaviour
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
    public void ReceiveHit(float amount, bool upAttack)
    {
        if (upAttack && _playerCombat.IsDodgingUp() || !upAttack && _playerCombat.IsDodgingDown()) return; 

        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            _isDead = true;
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), 0.5f);
        }
    }

    private void Respawn()
    {
        transform.position = new Vector3(-5, 0, 0);
        gameObject.SetActive(true);
    }
}
