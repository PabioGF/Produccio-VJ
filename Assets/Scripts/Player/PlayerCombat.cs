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
    [SerializeField] private GameObject _hurtbox;
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _dodgeCd;

    private PlayerInputActions _playerInputActions;
    private bool _isAttacking;
    private float _attackCdTimer;

    private bool _dodgeStance;
    private bool _isDodging;
    private float _dodgeCdTimer;
    private Animator _myAnimator;

    public enum DodgeType { HighDodge, LowDodge };
    private DodgeType _dodgeType;

    #endregion

    #region Unity methods
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Attack.performed += AttackInput;
        _myAnimator = GetComponent<Animator>();

        if (_attackArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Attack Area és null");
        }
        if (_hurtbox == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Hurtbox és null");
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Attack.performed -= AttackInput;
    }

    void Start()
    {
        _attackArea.SetActive(false);
    }

    void Update()
    {
        HandleTimers();
        HandleDodge();
    }
    #endregion

    private void HandleTimers()
    {
        if (!_isAttacking) _attackCdTimer += Time.deltaTime;
        if (!_isDodging) _dodgeCdTimer += Time.deltaTime;
    }

    /// <summary>
    /// Handles the dodge logic when holding the dodge trigger buttond and moving to the upper or lower direction axis
    /// </summary>
    private void HandleDodge()
    {
        if (_playerInputActions.Player.DodgeTrigger.ReadValue<float>() == 1) _dodgeStance = true;
        else _dodgeStance = false;

        if (_dodgeStance)
        {
            float dodgeDirection = _playerInputActions.Player.DodgeInput.ReadValue<float>();

            if (_dodgeCdTimer > _dodgeCd)
            {
                if (dodgeDirection == 1)
                {
                    ExecuteDodge(DodgeType.HighDodge);
                }
                if (dodgeDirection == -1)
                {
                    ExecuteDodge(DodgeType.LowDodge);
                }
            }
        }
    }

    /// <summary>
    /// Executes the attack coroutine when the button is pressed and the player is not pressing the dodge trigger button
    /// </summary>
    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && !_dodgeStance)
        {
            Debug.Log("Attack");
            ExecuteAttack();
        }
    }

    /// <summary>
    /// Activates the attack area during the set time and disables it afterwards
    /// </summary>
    public void ExecuteAttack()
    {
        _myAnimator.SetTrigger("startAttack");
        _isAttacking = true;
        _attackCdTimer = 0;
    }

    private void ExecuteDodge(DodgeType dodgeType)
    {
        Debug.Log("Dodging");
        _dodgeCdTimer = 0;
        _isDodging = true;
        _dodgeType = dodgeType;
        Invoke(nameof(StopDodge), _dodgeDuration);
    }

    /// <summary>
    /// Determines the dodging direction of the player and deactivates it after the set time has passed
    /// </summary>
    public void StopDodge()
    {
        _isDodging = false;
    }

    /// <summary>
    /// Returns the dodgeStance bool
    /// </summary>
    public bool DodgeStance()
    {
        return _dodgeStance;
    }

    public void OnDodge()
    {
        Debug.Log("Dodged");
    }

    private void EnableAttackArea()
    {
        _attackArea.SetActive(true);
    }

    private void DIsableAttackArea()
    {
        _attackArea.SetActive(false);
    }

    public DodgeType GetDodgeType => _dodgeType;

    public bool IsDodging => _isDodging;

}