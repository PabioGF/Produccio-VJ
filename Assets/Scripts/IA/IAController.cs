using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IAController : MonoBehaviour
{
    #region Global Variables
    [SerializeField] protected Transform _player;
    [SerializeField] private Transform leftFoot, rightFoot;

    [Header("Shared Enemy Movement Params")]
    [SerializeField] private float tiempoCambioDireccion;
    [SerializeField] private float _detectionExtraTime;
    [SerializeField] private float _velocityMultiplier;
    [SerializeField] protected float velocidadMovimiento;
    [SerializeField] protected bool _limitedMovement;
    [SerializeField] protected float _maxPosition, _minPosition;

    [Header("Shared Enemy Combat Params")]
    [SerializeField] protected float _detectionDistance;
    [SerializeField] protected float _attackRange;
    [SerializeField] private float _enablingDistance;
    [SerializeField] private int _scoreAddWhenDead;
    [SerializeField] private float _dropChance;
    [SerializeField] private GameObject[] _possibleDrops;

    protected float tiempoActual;
    protected int direccion = 1;
    protected bool hasDetected = false;
    protected Vector3 myVelocity;
    protected bool _isAttacking;
    protected bool _isFlying;

    protected Rigidbody2D myRB;
    protected Animator myAnimator;
    private float _detectionTimer;
    private bool _isHit;
    private float _lastTimeHit;
    private bool _isFlipped;
    private bool _isGrounded;
    private bool _canChangeDirection;
    #endregion

    #region Unity Methods
    protected virtual void Start()
    {
        ReiniciarTemporizador();
        hasDetected = false;
        myRB = GetComponent<Rigidbody2D>();
        _canChangeDirection = true;
    }
    
    protected virtual void Update()
    {
        if (Time.time - _lastTimeHit > 0.4)
        {
            ResetMovement();
        }
        IsGrounded();
    }
    #endregion

    #region Movement
    public virtual void EnemyBasicMovement()
    {
        if (DistanceToPlayer() > _enablingDistance) return;

        myVelocity = myRB.velocity;
        LookAtPlayer();
        DetectPlayer();
        IdleMove();
    }

    private void IsGrounded()
    {
        _isGrounded = CheckFeetColliding(leftFoot) || CheckFeetColliding(rightFoot);
        myAnimator.SetBool("IsGrounded", _isGrounded);
    }

    private void DetectPlayer()
    {
        // Lanzamos un rayo desde el enemigo hacia el jugador.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _player.position - transform.position, _detectionDistance, ~LayerMask.GetMask("Enemies", "Ignore Raycast", "Trigger"));
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, (_player.position - transform.position).normalized * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, (_player.position - transform.position).normalized * _detectionDistance, Color.red);
        }

        // Si el rayo no choca con ning�n objeto, devuelve true.
        if (hit.collider != null && hit.collider.CompareTag("Player") && !hasDetected)
        {
            hasDetected = true;
            velocidadMovimiento *= _velocityMultiplier;
        }

        // Stops chasing the player if it has not been detectend during the established time
        if ((hit.collider == null || !hit.collider.CompareTag("Player")) && hasDetected)
        {
            _detectionTimer += Time.deltaTime;
            if (_detectionTimer > _detectionExtraTime)
            {
                hasDetected = false;
                velocidadMovimiento /= _velocityMultiplier;
                _detectionTimer = 0;
            }
        }
    }

    private void IdleMove()
    {
        if (hasDetected) return;

        bool leftFootColliding = CheckFeetColliding(leftFoot);
        bool rightFootColliding = CheckFeetColliding(rightFoot);


        if (_canChangeDirection && (!leftFootColliding || !rightFootColliding || tiempoActual <= 0f || CheckMovementLimits()))
        {
            CambiarDireccion();
            ReiniciarTemporizador();
        }

        myVelocity.x = velocidadMovimiento * direccion;
        tiempoActual -= Time.deltaTime;
        myRB.velocity = myVelocity;
    }

    private void ResetDirectionChange()
    {
        _canChangeDirection = true;
    }

    private bool CheckMovementLimits()
    {
        if (!_limitedMovement) return false;
        return transform.position.x >= _maxPosition || transform.position.x <= _minPosition;
    }

    public void LookAtPlayer()
    {
        if (transform.position.x < _player.transform.position.x && !_isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isFlipped = true;
        }
        else if (transform.position.x > _player.transform.position.x && _isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isFlipped = false;
        }
    }

    private bool CheckFeetColliding(Transform feet)
    {
        return Physics2D.OverlapCircle(feet.position, 0.1f, LayerMask.GetMask("Ground"));
    }

    private void ReiniciarTemporizador()
    {
        tiempoActual = tiempoCambioDireccion;
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
        _canChangeDirection = false;
        Invoke(nameof(ResetDirectionChange), 0.5f);
    }

    private void ResetMovement()
    {
        myRB.gravityScale = 3;
    }
    #endregion

    public void GetHit()
    {
        _isHit = true;
        myRB.gravityScale = 0;
        myRB.velocity = new(0, 0);
        _lastTimeHit = Time.time;
    }

    public void StandStill()
    {
        Vector3 velocity = myRB.velocity;
        velocity.x = 0;
        myRB.velocity = velocity;
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(_player.position, myRB.position);
    }

    public void OnDeath()
    {
        GameController.Instance.AddScore(_scoreAddWhenDead);
        gameObject.SetActive(false);

        if (Random.value < _dropChance)    // Drops an item
        {
            GameObject drop = Instantiate(_possibleDrops[Random.Range(0, _possibleDrops.Length - 1)], transform.position, Quaternion.identity);

            if (drop.TryGetComponent(out BottleScript bottle))
            {
                Debug.Log("Set transform");
                bottle.SetPlayerRansform(_player);
            }
        }
            
    }

    public bool IsAttacking {  get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsFlying { get { return _isFlying; } set { _isFlying = value; } }
    public float AttackRange => _attackRange;

    public virtual void PlayHitSound()
    {
    }
}
