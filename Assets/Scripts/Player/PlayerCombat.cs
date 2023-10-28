using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    #region Variables
    [Header("Attack settings")]
    [SerializeField] private GameObject _attackArea;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;

    [Header("Dodge settings")]
    [SerializeField] private GameObject _dodgeArea;
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _dodgeCd;

    private PlayerInputActions _playerInputActions;
    private bool _isAttacking;
    private float _attackCdTimer;
    private bool _dodgeStance;
    private bool _isDodging;
    private float _dodgeCdTimer;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Attack.performed += AttackInput;

        if (_attackArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Attack Area és null");
        }
        if (_dodgeArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Dodge Area és null");
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Attack.performed -= AttackInput;
    }

    void Start()
    {
        _attackArea.SetActive(false); 
        _dodgeArea.SetActive(false);
    }

    void Update()
    {
        HandleTimers();
    }

    private void FixedUpdate()
    {
        HandleDodge();
    }
    #endregion

    private void HandleTimers()
    {
        if (!_isAttacking) { _attackCdTimer += Time.deltaTime; }
        if (!_isDodging) { _dodgeCdTimer += Time.deltaTime; }
    }

    private void HandleDodge()
    {
        if (_playerInputActions.Player.DodgeTrigger.ReadValue<float>() == 1) { _dodgeStance = true; }
        else { _dodgeStance = false; }

        if (_dodgeStance)
        {
            float dodgeDirection = _playerInputActions.Player.DodgeInput.ReadValue<float>();

            if (dodgeDirection == 1) 
            {
                Debug.Log("dodge Up");
                if (_dodgeCdTimer > _dodgeCd) { StartCoroutine(ExecuteDodge()); }
            }
            if (dodgeDirection == -1) 
            { 
                Debug.Log("Dodge down");
                if (_dodgeCdTimer > _dodgeCd) { StartCoroutine(ExecuteDodge()); }
            }
        }
    }

    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && !_dodgeStance)
        {
            if (_attackCdTimer > _attackCd) { StartCoroutine(ExecuteAttack()); }
        }
    }

    public IEnumerator ExecuteAttack()
    {
        _isAttacking = true;
        _attackArea.SetActive(true);
        _attackCdTimer = 0;

        yield return new WaitForSeconds(_attackDuration);

        _attackArea.SetActive(false);
        _isAttacking = false;
    }

    public IEnumerator ExecuteDodge()
    {
        _isDodging = true;
        _dodgeArea.SetActive(true);
        _dodgeCdTimer = 0;

        yield return new WaitForSeconds(_dodgeDuration);

        _dodgeArea.SetActive(false);
        _isDodging = false;
    }

    public bool DodgeStance()
    {
        return _dodgeStance;
    }
}