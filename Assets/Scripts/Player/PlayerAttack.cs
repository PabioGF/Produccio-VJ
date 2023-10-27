using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject _attackArea;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;

    private PlayerInputActions _playerInputActions;
    private bool _isAttacking;
    private float _attackDurationTimer;
    private float _attackCdTimer = 1;
    #endregion

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Attack.performed += AttackInput;

        if (_attackArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a attackArea és null");
        }
    }

    #region Unity methods
    void Start()
    {
        _attackArea.SetActive(false);  
    }

    void Update()
    {
        HandleAttack();
        HandleDodge();
    }
    #endregion

    private void HandleAttack()
    {
        if (_isAttacking)
        {
            _attackDurationTimer += Time.deltaTime;

            if (_attackDurationTimer >= _attackDuration)
            {
                _isAttacking = false;
                _attackArea.SetActive(_isAttacking);
                _attackDurationTimer = 0;
                _attackCdTimer = 0;
            }
        }
        else
        {
            _attackCdTimer += Time.deltaTime;
        }
    }

    private void HandleDodge()
    {

    }

    public void AttackInput(InputAction.CallbackContext context)
    {
        if (_attackCdTimer > _attackCd)
        {
            _attackArea.SetActive(_isAttacking);
            _isAttacking = true;
        }
    }
}
