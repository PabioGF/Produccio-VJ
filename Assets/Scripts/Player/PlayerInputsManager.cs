using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    public static PlayerInputsManager Instance;

    [Header("Scripts References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerCombat _playerCombat;

    [Header("Input Actions")]
    [SerializeField] private InputAction _horizontalInput;
    [SerializeField] private InputAction _verticalInput;
    [SerializeField] private InputAction _jump;
    [SerializeField] private InputAction _interact;
    [SerializeField] private InputAction _fastAttack;
    [SerializeField] private InputAction _slowAttack;
    [SerializeField] private InputAction _dash;
    [SerializeField] private InputAction _throw;
    [SerializeField] private InputAction _pause;

    public InputDevices InputDevice { get; private set; }
    public enum InputDevices { Keyboard, Controller };

    private int _interactionFramesCounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Actions subscribing
        _dash.performed += DashInput;
        _jump.performed += JumpInput;
        _interact.performed += InteractInput;
        _fastAttack.performed += FastAttackInput;
        _slowAttack.performed += SlowAttackInput;
        _throw.performed += ThrowInput;
        _pause.performed += PauseInput;

        InputSystem.onActionChange += InputSystem_onActionChange;
    }

    private void Update()
    {
        ManageInteraction();
    }

    private void InputSystem_onActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction lastInputAction = (InputAction)obj;
            InputDevice lastInputDevice = lastInputAction.activeControl.device;

            if (lastInputDevice.name.Equals("Keyboard") || lastInputDevice.name.Equals("Mouse"))
            {
                Debug.Log("Keyboard");
                InputDevice = InputDevices.Keyboard;
            }
            else
            {
                Debug.Log("Controller");
                InputDevice = InputDevices.Controller;
            }
        }
    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
        _interact.Disable();

        _dash.performed -= DashInput;
        _jump.performed -= JumpInput;
        _interact.performed -= InteractInput;
        _fastAttack.performed -= FastAttackInput;
        _slowAttack.performed -= SlowAttackInput;
        _throw.performed -= ThrowInput;
        _pause.performed -= PauseInput;
    }

    private void ManageInteraction()
    {
        if (_playerController.DesiredInteraction)
        {
            _interactionFramesCounter++;
            if (_interactionFramesCounter > 10)
            {
                _playerController.DesiredInteraction = false;
            }
        }
        else
        {
            _interactionFramesCounter = 0;
        }
    }
    
    public float ReadHorizontalInput()
    {
        return _horizontalInput.ReadValue<float>();
    }

    public float ReadVerticalInput()
    {
        return _verticalInput.ReadValue<float>();
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.HandleDashInput();
        }
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.HandleJumpInput();
        }
    }

    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerController.DesiredInteraction = true;
        }
    }

    public void FastAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleFastAttackInput();
        }
    }

    public void SlowAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleSlowAttackInput();
        }
    }

    public void ThrowInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerCombat.HandleThrowBottleInput();
        }
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIController.Instance.HandlePauseInput();
        }
    }

    public void EnableControls()
    {
        _horizontalInput.Enable();
        _verticalInput.Enable();
        _jump.Enable();
        _interact.Enable();
        _fastAttack.Enable();
        _slowAttack.Enable();
        _dash.Enable();
        _throw.Enable();
        _pause.Enable();
    }

    public void DisableControls()
    {
        _horizontalInput.Disable();
        _verticalInput.Disable();
        _jump.Disable();
        _fastAttack.Disable();
        _slowAttack.Disable();
        _dash.Disable();
        _throw.Disable();
        _pause.Disable();
    }
}
