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
    private bool _isDodgingUp;
    private bool _isDodgingDown;
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
    }

    private void FixedUpdate()
    {
        HandleDodge();
    }
    #endregion

    private void HandleTimers()
    {
        if (!_isAttacking) { _attackCdTimer += Time.deltaTime; }
        if (!_isDodgingUp && !_isDodgingDown) { _dodgeCdTimer += Time.deltaTime; }
    }

    /// <summary>
    /// Handles the dodge logic when holding the dodge trigger buttond and moving to the upper or lower direction axis
    /// </summary>
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
                if (_dodgeCdTimer > _dodgeCd) { StartCoroutine(ExecuteDodge(true)); }
            }
            if (dodgeDirection == -1) 
            { 
                Debug.Log("Dodge down");
                if (_dodgeCdTimer > _dodgeCd) { StartCoroutine(ExecuteDodge(false)); }
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
            if (_attackCdTimer > _attackCd) { StartCoroutine(ExecuteAttack()); }
        }
    }

    /// <summary>
    /// Activates the attack area during the set time and disables it afterwards
    /// </summary>
    public IEnumerator ExecuteAttack()
    {
        _isAttacking = true;
        _attackArea.SetActive(true);
        _attackCdTimer = 0;

        yield return new WaitForSeconds(_attackDuration);

        _attackArea.SetActive(false);
        _isAttacking = false;
    }

    /// <summary>
    /// Determines the dodging direction of the player and deactivates it after the set time has passed
    /// </summary>
    public IEnumerator ExecuteDodge(bool upDodge)
    {
        if (upDodge) { _isDodgingUp = true; }
        else { _isDodgingDown = true;}
        _dodgeCdTimer = 0;

        yield return new WaitForSeconds(_dodgeDuration);

        if (upDodge) { _isDodgingUp = false; }
        else { _isDodgingDown = false; }
    }

    /// <summary>
    /// Returns the dodgeStance bool
    /// </summary>
    public bool DodgeStance()
    {
        return _dodgeStance;
    }

    /// <summary>
    /// Handles all the collision interactions
    /// </summary>
    public void OnHurtboxTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyCollisionHandler(collision);
        }
    }

    /// <summary>
    /// Handles all the enemy collisions
    /// </summary>
    private void EnemyCollisionHandler(Collider2D collision)
    {
        if (_isDodgingUp)
        {
            if (collision.GetComponent<TestDodge>().GetAttackType()) { Debug.Log("Dodged"); }
            else { Debug.Log("Enemy Hit"); }
        }
        else if (_isDodgingDown)
        {
            if (collision.GetComponent<TestDodge>().GetAttackType()) { Debug.Log("Enemy Hit"); }
            else { Debug.Log("Dodged"); }
        }
        else
        {
            Debug.Log("Enemy Hit");
        }
    }
}